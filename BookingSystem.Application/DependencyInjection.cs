using BookingSystem.Application.Auth;
using BookingSystem.Application.Reservations;
using BookingSystem.Application.Restaurants;
using BookingSystem.Application.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace BookingSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ITableService, TableService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}