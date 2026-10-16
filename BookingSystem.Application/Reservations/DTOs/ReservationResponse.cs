using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Reservations.DTOs;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartTime { get; set; } // UTC
    public DateTime EndTime { get; set; } // UTC
    public int GuestsCount { get; set; }
    public ReservationStatus Status { get; set; }
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
