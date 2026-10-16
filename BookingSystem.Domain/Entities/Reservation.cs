using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Domain.Entities;

public class Reservation : BaseEntity
{
    public Guid TableId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartTime { get; set; }           // UTC
    public DateTime EndTime { get; set; }             // UTC
    public int GuestsCount { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? SpecialRequests { get; set; }

    public RestaurantTable? Table { get; set; }
    public User? Customer { get; set; }
}