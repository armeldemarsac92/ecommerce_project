using Stripe;
using Tdev702.Api.Endpoints.Shop;
using Tdev702.Api.Services;

namespace Tdev702.Api.DI;

public static class EndpointExtensions
{
    private const string StripeApiKey = "sk_test_51QgrmyIDr2BcEMc04bwDsB9bFbfHa3pFNk7DWcWGkg5BB04bCXQpvaVjUMvVGJTEp3jS0Qt7hiTysmwI2T9kBCVs00nDUXry0A";

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
        // Configure Stripe
        StripeConfiguration.ApiKey = StripeApiKey;

        // Register Stripe services
        services.AddScoped<InvoiceService>();
        services.AddScoped<IInvoicesService, InvoicesService>();
        
        // Register other services
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        
        return services;
    }
}