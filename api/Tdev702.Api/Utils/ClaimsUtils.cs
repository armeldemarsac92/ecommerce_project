using System.Security.Claims;

namespace Tdev702.Api.Utils;

public static class ClaimsUtils
{
    public static string? GetUserIdFromClaims(this HttpContext context)
    {
        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;
        return userId;
    }
    
    public static string GetUserRoleFromClaims(this HttpContext context)
    {
        var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userRole))
            throw new InvalidOperationException("User role not found in claims");
        return userRole;
    }
    
    public static string GetUserStripeIdFromClaims(this HttpContext context)
    {
        var stripeId = context.User.FindFirst(c => c.Type == "stripe_id")?.Value;
        if (string.IsNullOrEmpty(stripeId))
            throw new InvalidOperationException("Stripe ID not found in claims");
        return stripeId;
    }
}