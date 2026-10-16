namespace BookingSystem.Application.Reservations.DTOs;

public class AvailabilityRequest
{
    public Guid RestaurantId { get; set; }
    public DateTime StartTime { get; set; } // UTC
    public int GuestsCount { get; set; }
}
