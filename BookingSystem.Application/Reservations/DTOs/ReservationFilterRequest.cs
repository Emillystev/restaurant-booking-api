using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Reservations.DTOs;

public class ReservationFilterRequest
{
    public Guid? RestaurantId { get; set; }
    public Guid? TableId { get; set; }
    public Guid? CustomerId { get; set; }
    public ReservationStatus? Status { get; set; }
    public DateTime? StartTimeFrom { get; set; } // UTC
    public DateTime? StartTimeTo { get; set; } // UTC
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
