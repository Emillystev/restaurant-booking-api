using BookingSystem.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class ClosedDate : BaseEntity
{
    public Guid RestaurantId { get; set; }
    public DateOnly Date { get; set; }
    public string? Reason { get; set; }
    public Restaurant? Restaurant { get; set; }
}