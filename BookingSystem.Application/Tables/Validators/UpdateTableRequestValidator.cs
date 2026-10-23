using BookingSystem.Application.Tables.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Tables.Validators;

public class UpdateTableRequestValidator : AbstractValidator<UpdateTableRequest>
{
    public UpdateTableRequestValidator()
    {
        RuleFor(request => request.ZoneId)
            .NotEmpty();

        RuleFor(request => request.Number)
            .GreaterThan(0);

        RuleFor(request => request.Capacity)
            .GreaterThan(0);

        RuleFor(request => request.RowVersion)
            .Must(version => version is null || version.Length == 8)
            .WithMessage("RowVersion must contain 8 bytes when provided.");
    }
}
