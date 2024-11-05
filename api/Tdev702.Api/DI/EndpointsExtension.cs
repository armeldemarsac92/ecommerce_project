using Tdev702.Api.Endpoints.Shop;
using Tdev702.Api.Services;

namespace Tdev702.Api.DI;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProductEndpoints();

        return app;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
        return services;
    }
}