using BookingSystem.Application.Tables.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Tables.Validators;

public class CreateTableRequestValidator : AbstractValidator<CreateTableRequest>
{
    public CreateTableRequestValidator()
    {
        RuleFor(request => request.ZoneId)
            .NotEmpty();

        RuleFor(request => request.Number)
            .GreaterThan(0);

        RuleFor(request => request.Capacity)
            .GreaterThan(0);
    }
}
