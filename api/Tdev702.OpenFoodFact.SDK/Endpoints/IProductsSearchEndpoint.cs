using Refit;
using Tdev702.Contracts.OpenFoodFact;
using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.OpenFoodFact.SDK.Routes;

namespace Tdev702.OpenFoodFact.SDK.Endpoints;

public interface IProductsSearchEndpoint
{
    [Get(OpenFoodFactRoutes.Search.Product)]
    Task<ApiResponse<OpenFoodFactSearchResult>> SearchProducts([Query] ProductSearchParams searchParams, CancellationToken cancellationToken = default);
    Task<ApiResponse<OpenFoodFactProduct>> SearchProduct([Query] string productBarCode, CancellationToken cancellationToken = default);
}