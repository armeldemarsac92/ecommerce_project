using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface ITagEndpoints
{
    [Get(ShopRoutes.ProductsTags.GetAll)]
    Task<ApiResponse<List<TagResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.ProductsTags.GetById)]
    Task<ApiResponse<TagResponse>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.ProductsTags.Create)]
    Task<ApiResponse<TagResponse>> CreateAsync([Body] CreateTagRequest createTagRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.ProductsTags.Update)]
    Task<ApiResponse<TagResponse>> UpdateAsync(long id, [Body] UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default);
    
    [Delete(ShopRoutes.ProductsTags.Delete)]
    Task<IApiResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
}