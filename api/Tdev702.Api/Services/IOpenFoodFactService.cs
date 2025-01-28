using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.OpenFoodFact.SDK.Endpoints;

namespace Tdev702.Api.Services;

public interface IOpenFoodFactService
{
    Task<OpenFoodFactSearchResult> SearchProductAsync(ProductSearchParams searchParams, CancellationToken cancellationToken = default);
}

public class OpenFoodFactService : IOpenFoodFactService
{
    private readonly IProductsSearchEndpoint _productsSearchEndpoint;
    private readonly ILogger<OpenFoodFactService> _logger;

    public OpenFoodFactService(IProductsSearchEndpoint productsSearchEndpoint, ILogger<OpenFoodFactService> logger)
    {
        _productsSearchEndpoint = productsSearchEndpoint;
        _logger = logger;
    }

    public async Task<OpenFoodFactSearchResult> SearchProductAsync(ProductSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for product with query options: {queryOptions}", searchParams);
        var response = await _productsSearchEndpoint.SearchProducts(searchParams, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                $"Failed to search for product with query options: {searchParams}, status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to search for product with query options: {searchParams}");
        }

        return response.Content;
    }
}