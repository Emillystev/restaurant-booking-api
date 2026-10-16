namespace BookingSystem.Application.Reservations.DTOs;

public class CreateReservationRequest
{
    public Guid TableId { get; set; }
    public DateTime StartTime { get; set; } // UTC
    public int GuestsCount { get; set; }
    public string? SpecialRequests { get; set; }
}
