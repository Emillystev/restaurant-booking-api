using BookingSystem.Application.Auth.DTOs;

namespace BookingSystem.Application.Auth;

public class AuthService : IAuthService
{
    public Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
