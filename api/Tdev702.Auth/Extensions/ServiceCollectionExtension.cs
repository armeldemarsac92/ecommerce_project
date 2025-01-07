using Tdev702.Auth.Endpoints;

namespace Tdev702.Auth.Extensions;

public static class ServiceCollectionExtension
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRoleEndpoints();
        app.MapSecurityEndpoints();
        
        return app;
    }
}