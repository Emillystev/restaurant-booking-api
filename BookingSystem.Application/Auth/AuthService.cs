using BookingSystem.Application.Auth.DTOs;
using FluentValidation;

namespace BookingSystem.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RegisterRequest> _registerValidator;

    public AuthService(
        IValidator<LoginRequest> loginValidator,
        IValidator<RegisterRequest> registerValidator)
    {
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await _loginValidator.ValidateAndThrowAsync(request, ct);

        throw new NotImplementedException();
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await _registerValidator.ValidateAndThrowAsync(request, ct);

        throw new NotImplementedException();
    }
}
