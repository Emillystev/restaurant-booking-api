using BookingSystem.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class Restaurant : BaseEntity, ISoftDeletable
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public int ReservationDurationMinutes { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Zone>? Zones { get; set; }
    public ICollection<ClosedDate>? ClosedDates { get; set; }
}
