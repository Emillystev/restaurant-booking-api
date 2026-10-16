namespace BookingSystem.Application.Reservations.DTOs;

public class AvailableTableResponse
{
    public Guid Id { get; set; }
    public Guid ZoneId { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
}
