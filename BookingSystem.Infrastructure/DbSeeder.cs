using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Restaurants.IgnoreQueryFilters().AnyAsync())
            return;

        var admin = new User
        {
            Email = "admin@booking.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            FullName = "System Admin",
            Phone = "+995555000001",
            Role = UserRole.Admin
        };

        var customer = new User
        {
            Email = "customer@booking.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
            FullName = "Test Customer",
            Phone = "+995555000002",
            Role = UserRole.Customer
        };

        var restaurant = new Restaurant
        {
            Name = "Tbilisi Garden",
            Address = "Rustaveli Ave 10, Tbilisi",
            Phone = "+995322000000",
            OpenTime = new TimeOnly(10, 0),
            CloseTime = new TimeOnly(23, 0),
            ReservationDurationMinutes = 120
        };

        var indoor = new Zone { Restaurant = restaurant, ZoneName = ZoneName.Indoor };
        var terrace = new Zone { Restaurant = restaurant, ZoneName = ZoneName.Terrace };
        var vip = new Zone { Restaurant = restaurant, ZoneName = ZoneName.VIP };

        var tables = new List<RestaurantTable>
        {
            // Indoor
            new() { Zone = indoor, Number = 1, Capacity = 2 },
            new() { Zone = indoor, Number = 2, Capacity = 2 },
            new() { Zone = indoor, Number = 3, Capacity = 4 },
            new() { Zone = indoor, Number = 4, Capacity = 4 },
            new() { Zone = indoor, Number = 5, Capacity = 6 },

            // Terrace
            new() { Zone = terrace, Number = 1, Capacity = 2 },
            new() { Zone = terrace, Number = 2, Capacity = 4 },
            new() { Zone = terrace, Number = 3, Capacity = 6 },

            // VIP
            new() { Zone = vip, Number = 1, Capacity = 6 },
            new() { Zone = vip, Number = 2, Capacity = 8 },
        };

        db.Users.AddRange(admin, customer);
        db.Restaurants.Add(restaurant);
        db.Zones.AddRange(indoor, terrace, vip);
        db.Tables.AddRange(tables);

        await db.SaveChangesAsync();
    }
}
