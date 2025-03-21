using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface IOpenFoodFactEndpoints
{
    [Get(ShopRoutes.OpenFoodFactProducts.GetAll)]
    Task<ApiResponse<OpenFoodFactSearchResult>> GetAllAsync([Query] ProductSearchParams options, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.OpenFoodFactProducts.GetByBarcode)]
    Task<ApiResponse<OpenFoodFactProductResult>> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
}