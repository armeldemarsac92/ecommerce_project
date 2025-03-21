using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface ICategoryEndpoints
{
    [Get(ShopRoutes.Categories.GetAll)]
    Task<ApiResponse<List<CategoryResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Categories.GetById)]
    Task<ApiResponse<CategoryResponse>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.Categories.Create)]
    Task<ApiResponse<CategoryResponse>> CreateAsync([Body] CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Categories.Update)]
    Task<ApiResponse<CategoryResponse>> UpdateAsync(long id, [Body] UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    
    [Delete(ShopRoutes.Categories.Delete)]
    Task<IApiResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
}