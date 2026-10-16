using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Domain.Entities;

public class Zone : BaseEntity
{
    public Guid RestaurantId { get; set; }
    public ZoneName ZoneName { get; set; }
    public Restaurant? Restaurant { get; set; }
    public ICollection<RestaurantTable>? Tables { get; set; }
}
