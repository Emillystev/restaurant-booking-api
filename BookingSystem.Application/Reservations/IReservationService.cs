using BookingSystem.Application.Common.Models;
using BookingSystem.Application.Reservations.DTOs;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Reservations;

public interface IReservationService
{
    Task<List<AvailableTableResponse>> GetAvailableTablesAsync(
        AvailabilityRequest request, CancellationToken ct = default);

    Task<ReservationResponse> CreateAsync(
        CreateReservationRequest request, CancellationToken ct = default);

    Task CancelAsync(Guid reservationId, CancellationToken ct = default);

    Task<PagedResult<ReservationResponse>> GetMyReservationsAsync(
        int page, int pageSize, CancellationToken ct = default);

    Task<PagedResult<ReservationResponse>> GetAllAsync(
        ReservationFilterRequest request, CancellationToken ct = default);

    Task ChangeStatusAsync(
        Guid reservationId, ReservationStatus status, CancellationToken ct = default);
}
