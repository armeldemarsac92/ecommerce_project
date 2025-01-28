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
        app.MapInventoryEndpoints();
        app.MapOrderEndpoints();
        app.MapPaymentEndpoints();
        app.MapPaymentMethodEndpoints();
        app.MapWebhookEndpoint();
        app.MapCustomerEndpoints();
        app.MapTagEndpoints();
        app.MapOpenFoodFactEndpoints();
        return app;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<ITagsService, TagsService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IInventoriesService, InventoriesService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOpenFoodFactService, OpenFoodFactService>();
        return services;
    }
}