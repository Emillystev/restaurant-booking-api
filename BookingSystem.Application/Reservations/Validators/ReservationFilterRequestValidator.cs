using BookingSystem.Application.Reservations.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Reservations.Validators;

public class ReservationFilterRequestValidator : AbstractValidator<ReservationFilterRequest>
{
    public ReservationFilterRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than zero.");

        RuleFor(request => request.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than zero.");

        RuleFor(request => request.RestaurantId)
            .NotEqual(Guid.Empty)
            .WithMessage("RestaurantId must not be an empty GUID when provided.")
            .When(request => request.RestaurantId.HasValue);

        RuleFor(request => request.TableId)
            .NotEqual(Guid.Empty)
            .WithMessage("TableId must not be an empty GUID when provided.")
            .When(request => request.TableId.HasValue);

        RuleFor(request => request.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage("CustomerId must not be an empty GUID when provided.")
            .When(request => request.CustomerId.HasValue);

        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid reservation status.")
            .When(request => request.Status.HasValue);

        RuleFor(request => request.StartTimeFrom)
            .Must(startTime => !startTime.HasValue || startTime.Value.Kind == DateTimeKind.Utc)
            .WithMessage("StartTimeFrom must be specified in UTC.");

        RuleFor(request => request.StartTimeTo)
            .Must(startTime => !startTime.HasValue || startTime.Value.Kind == DateTimeKind.Utc)
            .WithMessage("StartTimeTo must be specified in UTC.");

        RuleFor(request => request.StartTimeTo)
            .Must((request, end) => end!.Value >= request.StartTimeFrom!.Value)
            .When(request => request.StartTimeFrom.HasValue && request.StartTimeTo.HasValue)
            .WithMessage("StartTimeTo must be greater than or equal to StartTimeFrom.");
    }
}
