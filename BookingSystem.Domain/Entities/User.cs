using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Domain.Entities;

public class User : BaseEntity
{
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public ICollection<Reservation>? Reservations { get; set; }
}
