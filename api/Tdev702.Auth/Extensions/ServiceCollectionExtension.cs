using Tdev702.Auth.Endpoints;

namespace Tdev702.Auth.Extensions;

public static class ServiceCollectionExtension
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSecurityEndpoints();
        app.MapRoleEndpoints();
        
        var endpoints = app.DataSources.SelectMany(ds => ds.Endpoints).OfType<RouteEndpoint>();
        foreach (var endpoint in endpoints)
        {
            Console.WriteLine($"Registered: {endpoint.RoutePattern.RawText} - Methods: {string.Join(", ", endpoint.Metadata.OfType<HttpMethodMetadata>().SelectMany(m => m.HttpMethods))}");
        }
        
        return app;
    }
}