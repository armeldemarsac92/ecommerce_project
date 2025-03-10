using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.OpenFoodFact.SDK.Endpoints;

namespace Tdev702.Api.Services;

public interface IOpenFoodFactService
{
    Task<OpenFoodFactSearchResult> SearchProductAsync(ProductSearchParams searchParams, CancellationToken cancellationToken = default);
    Task<OpenFoodFactProduct?> GetProductByBarCodeAsync(string barcode, CancellationToken cancellationToken = default);
}

public class OpenFoodFactService : IOpenFoodFactService
{
    private readonly IProductSearchEndpoints _productSearchEndpoints;
    private readonly ILogger<OpenFoodFactService> _logger;

    public OpenFoodFactService(IProductSearchEndpoints productSearchEndpoints, ILogger<OpenFoodFactService> logger)
    {
        _productSearchEndpoints = productSearchEndpoints;
        _logger = logger;
    }

    public async Task<OpenFoodFactSearchResult> SearchProductAsync(ProductSearchParams searchParams,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for product with query options: {queryOptions}", searchParams);
        var response = await _productSearchEndpoints.SearchProducts(searchParams, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Failed to search for product with query options: {searchParams}, status code: {StatusCode}", searchParams, response.StatusCode);
            throw new HttpRequestException($"Failed to search for product with query options: {searchParams}");
        }

        return response.Content;
    }

    public async Task<OpenFoodFactProduct?> GetProductByBarCodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting open food fact product by barcode: {barcode}", barcode);
        var response = await _productSearchEndpoints.GetProductByBarCode(barcode, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get product by barcode: {barcode}, status code: {StatusCode}", barcode, response.StatusCode);
            throw new HttpRequestException($"Failed to get product by barcode: {barcode}");
        }
        
        return response.Content.Product;
        
    }
}