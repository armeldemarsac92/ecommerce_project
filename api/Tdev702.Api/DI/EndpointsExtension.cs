using Tdev702.Api.Endpoints;
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
        app.MapOrderEndpoints();
        app.MapPaymentEndpoints();
        app.MapPaymentMethodEndpoints();
        app.MapWebhookEndpoint();
        return app;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IProductTagsService, ProductTagsService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}