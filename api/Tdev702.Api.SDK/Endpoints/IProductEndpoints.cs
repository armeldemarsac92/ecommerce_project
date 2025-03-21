using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface IProductEndpoints
{
    [Get(ShopRoutes.Products.GetAll)]
    Task<ApiResponse<List<ShopProductResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Products.GetById)]
    Task<ApiResponse<ShopProductResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Products.GetLiked)]
    Task<ApiResponse<List<ShopProductResponse>>> GetLikedProductsAsync(string? pageSize = null, string? pageNumber = null, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.Products.Create)]
    Task<ApiResponse<ShopProductResponse>> CreateAsync([Body] CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Products.Update)]
    Task<ApiResponse<ShopProductResponse>> UpdateAsync(int id, [Body] UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    
    [Delete(ShopRoutes.Products.Delete)]
    Task<ApiResponse<ShopProductResponse>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Products.Like)]
    Task LikeAsync(int id, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Products.Unlike)]
    Task UnlikeAsync(int id, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.HealthCheck)]
    Task<ApiResponse<string>> HealthCheckAsync(CancellationToken cancellationToken = default);
}