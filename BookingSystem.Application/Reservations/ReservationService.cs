using BookingSystem.Application.Common.Models;
using BookingSystem.Application.Reservations.DTOs;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Application.Reservations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db)
    {
        _db = db;
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

    public Task<List<AvailableTableResponse>> GetAvailableTablesAsync(AvailabilityRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ReservationResponse>> GetMyReservationsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
