using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Tdev702.OpenFoodFact.SDK.Extensions;

public static class DI
{
    public static IServiceCollection AddOpenFoodFactServices(this IServiceCollection services)
    {
        services.AddRefitClient<IProductService>(options =>)
        services.AddScoped<IProductService, OpenFoodFactProductService>();
        return services;
    }
}