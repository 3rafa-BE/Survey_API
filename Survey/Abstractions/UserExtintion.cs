using System.Security.Claims;

namespace Survey.Abstractions
{
    public static class UserExtintion
    {
        public static string? GetUserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
