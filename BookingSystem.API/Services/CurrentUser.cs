using System.Security.Claims;
using BookingSystem.Application.Common.Interfaces;

namespace BookingSystem.API.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

            if (identity?.IsAuthenticated != true)
            {
                return null;
            }

            var value = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? identity.FindFirst("sub")?.Value;

            return Guid.TryParse(value, out var userId) && userId != Guid.Empty
                ? userId
                : null;
        }
    }
}
