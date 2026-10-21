using BookingSystem.Application.Reservations.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Reservations.Validators;

public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(request => request.TableId)
            .NotEmpty()
            .WithMessage("TableId is required.");

        RuleFor(request => request.StartTime)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("StartTime is required.")
            .GreaterThan(_ => DateTime.UtcNow)
            .WithMessage("StartTime must be in the future.");

        RuleFor(request => request.GuestsCount)
            .GreaterThan(0)
            .WithMessage("GuestsCount must be greater than zero.");

        RuleFor(request => request.SpecialRequests)
            .MaximumLength(500)
            .WithMessage("SpecialRequests must not exceed 500 characters.");
    }
}
