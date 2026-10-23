using BookingSystem.Application.Restaurants.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Restaurants.Validators;

public class CreateRestaurantRequestValidator : AbstractValidator<CreateRestaurantRequest>
{
    public CreateRestaurantRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(request => request.Address)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(request => request.Phone)
            .MaximumLength(30);

        // Closing before opening represents an overnight schedule; midnight is valid.
        RuleFor(request => request.CloseTime)
            .NotEqual(request => request.OpenTime)
            .WithMessage("CloseTime must differ from OpenTime.");

        RuleFor(request => request.ReservationDurationMinutes)
            .GreaterThan(0);
    }
}
