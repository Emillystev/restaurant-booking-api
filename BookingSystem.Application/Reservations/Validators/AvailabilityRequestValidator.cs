using BookingSystem.Application.Reservations.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Reservations.Validators;

public class AvailabilityRequestValidator : AbstractValidator<AvailabilityRequest>
{
    public AvailabilityRequestValidator()
    {
        RuleFor(request => request.RestaurantId)
            .NotEmpty()
            .WithMessage("RestaurantId is required.");

        RuleFor(request => request.StartTime)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("StartTime is required.")
            .GreaterThan(_ => DateTime.UtcNow)
            .WithMessage("StartTime must be in the future.");

        RuleFor(request => request.GuestsCount)
            .GreaterThan(0)
            .WithMessage("GuestsCount must be greater than zero.");
    }
}
