using BookingSystem.Application.Restaurants.DTOs;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Application.Restaurants;

public class RestaurantService : IRestaurantService
{
    private readonly AppDbContext _db;

    public RestaurantService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddClosedDateAsync(Guid restaurantId, DateOnly date, string? reason, CancellationToken ct = default)
    {
        if (reason is not null && reason.Length > 200)
        {
            throw new ArgumentException("Reason cannot exceed 200 characters.", nameof(reason));
        }

        if (!await _db.Restaurants.AnyAsync(restaurant => restaurant.Id == restaurantId, ct))
        {
            throw new KeyNotFoundException($"Restaurant with id '{restaurantId}' was not found.");
        }

        if (await _db.ClosedDates.AnyAsync(closedDate =>
            closedDate.RestaurantId == restaurantId && closedDate.Date == date, ct))
        {
            throw new InvalidOperationException(
                $"Restaurant with id '{restaurantId}' already has a closed date on '{date:yyyy-MM-dd}'.");
        }

        var closedDate = new ClosedDate
        {
            RestaurantId = restaurantId,
            Date = date,
            Reason = reason
        };

        _db.ClosedDates.Add(closedDate);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var restaurant = new Restaurant
        {
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime,
            ReservationDurationMinutes = request.ReservationDurationMinutes
        };

        _db.Restaurants.Add(restaurant);
        await _db.SaveChangesAsync(ct);

        return new RestaurantResponse
        {
            Id = restaurant.Id,
            Name = restaurant.Name ?? string.Empty,
            Address = restaurant.Address ?? string.Empty,
            Phone = restaurant.Phone,
            OpenTime = restaurant.OpenTime,
            CloseTime = restaurant.CloseTime,
            ReservationDurationMinutes = restaurant.ReservationDurationMinutes,
            CreatedAt = restaurant.CreatedAt,
            UpdatedAt = restaurant.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var restaurant = await _db.Restaurants
            .SingleOrDefaultAsync(restaurant => restaurant.Id == id, ct)
            ?? throw new KeyNotFoundException($"Restaurant with id '{id}' was not found.");

        restaurant.IsDeleted = true;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<RestaurantResponse>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Restaurants
            .AsNoTracking()
            .OrderBy(restaurant => restaurant.Name)
            .ThenBy(restaurant => restaurant.Id)
            .Select(restaurant => new RestaurantResponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name ?? string.Empty,
                Address = restaurant.Address ?? string.Empty,
                Phone = restaurant.Phone,
                OpenTime = restaurant.OpenTime,
                CloseTime = restaurant.CloseTime,
                ReservationDurationMinutes = restaurant.ReservationDurationMinutes,
                CreatedAt = restaurant.CreatedAt,
                UpdatedAt = restaurant.UpdatedAt
            })
            .ToListAsync(ct);
    }

    public async Task<RestaurantResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var restaurant = await _db.Restaurants
            .AsNoTracking()
            .SingleOrDefaultAsync(restaurant => restaurant.Id == id, ct)
            ?? throw new KeyNotFoundException($"Restaurant with id '{id}' was not found.");

        return new RestaurantResponse
        {
            Id = restaurant.Id,
            Name = restaurant.Name ?? string.Empty,
            Address = restaurant.Address ?? string.Empty,
            Phone = restaurant.Phone,
            OpenTime = restaurant.OpenTime,
            CloseTime = restaurant.CloseTime,
            ReservationDurationMinutes = restaurant.ReservationDurationMinutes,
            CreatedAt = restaurant.CreatedAt,
            UpdatedAt = restaurant.UpdatedAt
        };
    }

    public async Task UpdateAsync(Guid id, UpdateRestaurantRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var restaurant = await _db.Restaurants
            .SingleOrDefaultAsync(restaurant => restaurant.Id == id, ct)
            ?? throw new KeyNotFoundException($"Restaurant with id '{id}' was not found.");

        restaurant.Name = request.Name;
        restaurant.Address = request.Address;
        restaurant.Phone = request.Phone;
        restaurant.OpenTime = request.OpenTime;
        restaurant.CloseTime = request.CloseTime;
        restaurant.ReservationDurationMinutes = request.ReservationDurationMinutes;

        await _db.SaveChangesAsync(ct);
    }
}
