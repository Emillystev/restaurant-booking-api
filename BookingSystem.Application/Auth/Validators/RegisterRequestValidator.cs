using BookingSystem.Application.Auth.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(request => request.FullName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(request => request.Phone)
            .MaximumLength(30);
    }
}
