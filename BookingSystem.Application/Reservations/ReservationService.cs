using Azure.Core;
using BookingSystem.Application.Common.Models;
using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Application.Reservations.DTOs;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Application.Reservations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public ReservationService(AppDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }
    public async Task CancelAsync(Guid reservationId, CancellationToken ct = default)
    {
        var reservation = await _db.Reservations
            .SingleOrDefaultAsync(reservation => reservation.Id == reservationId, ct)
            ?? throw new KeyNotFoundException($"Reservation with id '{reservationId}' was not found.");

        if (reservation.Status != ReservationStatus.Pending && reservation.Status != ReservationStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only pending or confirmed reservations can be cancelled.");
        }

        reservation.Status = ReservationStatus.Cancelled;

        await _db.SaveChangesAsync(ct);
    }

    public async Task ChangeStatusAsync(Guid reservationId, ReservationStatus status, CancellationToken ct = default)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentOutOfRangeException(nameof(status));
        }

        var reservation = await _db.Reservations
            .SingleOrDefaultAsync(reservation => reservation.Id == reservationId, ct)
            ?? throw new KeyNotFoundException($"Reservation with id '{reservationId}' was not found.");

        var isAllowed = reservation.Status switch
        {
            ReservationStatus.Pending =>
                status is ReservationStatus.Confirmed
                    or ReservationStatus.Cancelled,

            ReservationStatus.Confirmed =>
                status is ReservationStatus.Seated
                    or ReservationStatus.Cancelled
                    or ReservationStatus.NoShow,

            ReservationStatus.Seated =>
                status == ReservationStatus.Completed,

            _ => false
        };

        if (!isAllowed)
        {
            throw new InvalidOperationException(
                $"Cannot change status from '{reservation.Status}' to '{status}'.");
        }

        reservation.Status = status;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request, CancellationToken ct = default)
    {
        var reservation = new Reservation()
        {
            TableId = request.TableId,
            StartTime = request.StartTime,
            GuestsCount = request.GuestsCount,
            SpecialRequests = request.SpecialRequests,
            Status = ReservationStatus.Pending
        };

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync(ct);

        return new ReservationResponse
        {
            Id = reservation.Id,
            TableId = reservation.TableId,
            CustomerId = reservation.CustomerId,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            GuestsCount = reservation.GuestsCount,
            Status = reservation.Status,
            SpecialRequests = reservation.SpecialRequests,
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt
        };
    }

    public async Task<PagedResult<ReservationResponse>> GetAllAsync(ReservationFilterRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.Page);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.PageSize);

        var query = _db.Reservations
            .AsNoTracking()
            .Where(reservation =>
                (!request.RestaurantId.HasValue ||
                    (reservation.Table != null && reservation.Table.Zone.RestaurantId == request.RestaurantId)) &&
                (!request.TableId.HasValue || reservation.TableId == request.TableId) &&
                (!request.CustomerId.HasValue || reservation.CustomerId == request.CustomerId) &&
                (!request.Status.HasValue || reservation.Status == request.Status) &&
                (!request.StartTimeFrom.HasValue || reservation.StartTime >= request.StartTimeFrom) &&
                (!request.StartTimeTo.HasValue || reservation.StartTime <= request.StartTimeTo));

        var totalCount = await query.CountAsync(ct);
        var skip = checked((request.Page - 1) * request.PageSize);

        var items = await query
            .OrderByDescending(reservation => reservation.StartTime)
            .ThenBy(reservation => reservation.Id)
            .Skip(skip)
            .Take(request.PageSize)
            .Select(reservation => new ReservationResponse
            {
                Id = reservation.Id,
                TableId = reservation.TableId,
                CustomerId = reservation.CustomerId,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                GuestsCount = reservation.GuestsCount,
                Status = reservation.Status,
                SpecialRequests = reservation.SpecialRequests,
                CreatedAt = reservation.CreatedAt,
                UpdatedAt = reservation.UpdatedAt
            })
            .ToListAsync(ct);

        return new PagedResult<ReservationResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<AvailableTableResponse>> GetAvailableTablesAsync(AvailabilityRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.GuestsCount);

        if (request.RestaurantId == Guid.Empty)
        {
            throw new ArgumentException("RestaurantId is required.", nameof(request));
        }

        if (request.StartTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("StartTime must be specified in UTC.", nameof(request));
        }

        if (request.StartTime <= DateTime.UtcNow)
        {
            throw new ArgumentException("StartTime must be in the future.", nameof(request));
        }

        var restaurant = await _db.Restaurants
            .AsNoTracking()
            .SingleOrDefaultAsync(restaurant => restaurant.Id == request.RestaurantId, ct)
            ?? throw new KeyNotFoundException($"Restaurant with id '{request.RestaurantId}' was not found.");

        if (restaurant.ReservationDurationMinutes <= 0)
        {
            throw new InvalidOperationException("Restaurant reservation duration must be greater than zero.");
        }

        var startTime = request.StartTime;
        var endTime = startTime.AddMinutes(restaurant.ReservationDurationMinutes);
        var date = DateOnly.FromDateTime(startTime);
        var openingTime = date.ToDateTime(restaurant.OpenTime, DateTimeKind.Utc);
        var closingTime = date.ToDateTime(restaurant.CloseTime, DateTimeKind.Utc);

        // An overnight schedule belongs to the opening date, even after midnight.
        if (restaurant.CloseTime < restaurant.OpenTime)
        {
            closingTime = closingTime.AddDays(1);

            if (startTime < openingTime)
            {
                openingTime = openingTime.AddDays(-1);
                closingTime = closingTime.AddDays(-1);
            }
        }

        if (startTime < openingTime || endTime > closingTime)
        {
            return [];
        }

        // A booking ending at midnight does not occupy the following date.
        var lastOccupiedDate = DateOnly.FromDateTime(endTime.AddTicks(-1));

        if (await _db.ClosedDates.AnyAsync(closedDate =>
            closedDate.RestaurantId == request.RestaurantId &&
            closedDate.Date >= date && closedDate.Date <= lastOccupiedDate, ct))
        {
            return [];
        }

        return await _db.Tables
            .AsNoTracking()
            .Where(table =>
                table.Zone.RestaurantId == request.RestaurantId &&
                table.IsActive &&
                table.Capacity >= request.GuestsCount &&
                !_db.Reservations.Any(reservation =>
                    reservation.TableId == table.Id &&
                    (reservation.Status == ReservationStatus.Pending ||
                     reservation.Status == ReservationStatus.Confirmed ||
                     reservation.Status == ReservationStatus.Seated) &&
                    reservation.StartTime < endTime &&
                    reservation.EndTime > startTime))
            .OrderBy(table => table.Capacity)
            .ThenBy(table => table.Number)
            .ThenBy(table => table.Id)
            .Select(table => new AvailableTableResponse
            {
                Id = table.Id,
                ZoneId = table.ZoneId,
                Number = table.Number,
                Capacity = table.Capacity
            })
            .ToListAsync(ct);
    }

    public async Task<PagedResult<ReservationResponse>> GetMyReservationsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        var customerId = _currentUser.UserId;

        if (!customerId.HasValue || customerId.Value == Guid.Empty)
        {
            throw new UnauthorizedAccessException("An authenticated user with a valid user ID is required.");
        }

        return await GetAllAsync(new ReservationFilterRequest
        {
            CustomerId = customerId.Value,
            Page = page,
            PageSize = pageSize
        }, ct);
    }
}
