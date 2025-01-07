namespace Tdev702.Auth.Extensions;

public static class SecurityPolicies
{
    public static IServiceCollection AddSecurityPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder().AddPolicy("AdminPolicy", policy => 
        {
            policy.RequireRole("Admin")
                .RequireAuthenticatedUser();
        });
        
        return services;
    }
}