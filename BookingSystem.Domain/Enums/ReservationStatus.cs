namespace BookingSystem.Domain.Enums;

public enum ReservationStatus : byte
{
    Pending = 1,
    Confirmed = 2,
    Seated = 3,
    Completed = 4,
    Cancelled = 5,
    NoShow = 6
}
