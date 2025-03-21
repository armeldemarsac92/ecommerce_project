using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface IBrandEndpoints
{
    [Get(ShopRoutes.Brands.GetAll)]
    Task<ApiResponse<List<BrandResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Brands.GetById)]
    Task<ApiResponse<BrandResponse>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.Brands.Create)]
    Task<ApiResponse<BrandResponse>> CreateAsync([Body] CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Brands.Update)]
    Task<ApiResponse<BrandResponse>> UpdateAsync(long id, [Body] UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default);
    
    [Delete(ShopRoutes.Brands.Delete)]
    Task<IApiResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
}