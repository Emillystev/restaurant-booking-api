using BookingSystem.Application.Common.Models;
using BookingSystem.Application.Reservations.DTOs;
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

    public Task<ReservationResponse> CreateAsync(CreateReservationRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ReservationResponse>> GetAllAsync(ReservationFilterRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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
