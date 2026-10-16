using BookingSystem.Application.Restaurants.DTOs;

namespace BookingSystem.Application.Restaurants;

public interface IRestaurantService
{
    Task<List<RestaurantResponse>> GetAllAsync(CancellationToken ct = default);
    Task<RestaurantResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateRestaurantRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task AddClosedDateAsync(Guid restaurantId, DateOnly date, string? reason, CancellationToken ct = default);
}
