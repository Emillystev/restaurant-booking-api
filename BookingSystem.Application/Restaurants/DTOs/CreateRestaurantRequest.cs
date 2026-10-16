namespace BookingSystem.Application.Restaurants.DTOs;

public class CreateRestaurantRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public int ReservationDurationMinutes { get; set; }
}
