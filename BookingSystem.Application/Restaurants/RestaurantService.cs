using BookingSystem.Application.Restaurants.DTOs;

namespace BookingSystem.Application.Restaurants;

public class RestaurantService : IRestaurantService
{
    public Task AddClosedDateAsync(Guid restaurantId, DateOnly date, string? reason, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestaurantResponse> CreateAsync(CreateRestaurantRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RestaurantResponse>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestaurantResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, UpdateRestaurantRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
