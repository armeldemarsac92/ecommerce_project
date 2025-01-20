using Stripe;
using Tdev702.Api.Endpoints.Shop;
using Tdev702.Api.Services;

namespace Tdev702.Api.DI;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProductEndpoints();
        app.MapBrandEndpoints();
        app.MapCategoryEndpoints();
        app.MapInvoiceEndpoints();
        return app;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        
        return services;
    }
}