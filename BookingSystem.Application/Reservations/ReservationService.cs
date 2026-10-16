using BookingSystem.Application.Common.Models;
using BookingSystem.Application.Reservations.DTOs;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure;

namespace BookingSystem.Application.Reservations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db)
    {
        _db = db;
    }
    public Task CancelAsync(Guid reservationId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task ChangeStatusAsync(Guid reservationId, ReservationStatus status, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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
