using System.Security.Claims;

namespace SchoolSystem.Core.Extensions;

public static class ClaimsPrincipalExtensions {
    public static bool HasRole(this ClaimsPrincipal principal, string role) {
        return principal.IsInRole(role);
    }

    public static bool HasRole(this ClaimsPrincipal principal, string role1, string role2) {
        return principal.IsInRole(role1) || principal.IsInRole(role2);
    }

    public static bool HasAnyRole(this ClaimsPrincipal principal, params string[] roles) {
        return roles.Any(role => principal.IsInRole(role));
    }

    public static int GetUserId(this ClaimsPrincipal principal) {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    public static string GetUserName(this ClaimsPrincipal principal) {
        return principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}