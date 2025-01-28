using Microsoft.Extensions.DependencyInjection;
using Refit;
using Tdev702.OpenFoodFact.SDK.Endpoints;

namespace Tdev702.OpenFoodFact.SDK.Extensions;

public static class DI
{
    public static IServiceCollection AddOpenFoodFactServices(this IServiceCollection services)
    {
        services.AddRefitClient<IProductsSearchEndpoint>().ConfigureHttpClient(
            c =>
            {
                c.BaseAddress = new Uri("https://world.openfoodfacts.org");
                c.DefaultRequestHeaders.Add("User-Agent", "TDEV700/1.0 (armeldemarsac@gmail.com)");
            });
        return services;
    }
}