using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace Tdev702.Auth.Extensions;

public static class SecurityPolicies
{
    public static IServiceCollection AddSecurityPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder().AddPolicy("Admin", policy =>
            {
                policy.RequireRole("Admin")
                    .RequireAuthenticatedUser();
            })
            .AddPolicy("User", policy =>
            {
                policy.RequireRole("User")
                    .RequireAuthenticatedUser();
            })
            .AddPolicy("Authenticated", policy =>
            {
                policy.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        
        return services;
    }
}