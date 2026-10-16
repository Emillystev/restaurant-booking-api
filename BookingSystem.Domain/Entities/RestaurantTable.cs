using BookingSystem.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class RestaurantTable : BaseEntity
{
    public Guid ZoneId { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;
    public byte[]? RowVersion { get; set; }
    public Zone Zone { get; set; } = default!;
    public ICollection<Reservation>? Reservations { get; set; }
}