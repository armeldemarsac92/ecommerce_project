using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface IInventoryEndpoints
{
    [Get(ShopRoutes.Inventories.GetById)]
    Task<ApiResponse<InventoryResponse>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Inventories.GetInventoryByProductId)]
    Task<ApiResponse<InventoryResponse>> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Inventories.GetAll)]
    Task<ApiResponse<List<InventoryResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.Inventories.Create)]
    Task<ApiResponse<InventoryResponse>> CreateAsync([Body] CreateInventoryRequest createInventoryRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Inventories.Update)]
    Task<ApiResponse<InventoryResponse>> UpdateAsync(long id, [Body] UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default);
    
    [Delete(ShopRoutes.Inventories.Delete)]
    Task<IApiResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Inventories.IncreamentStockInventory)]
    Task<IApiResponse> IncrementStockAsync(long productId, [Body] UpdateQuantityRequest updateQuantityRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Inventories.SubstractStockInventory)]
    Task<IApiResponse> DecrementStockAsync(long productId, [Body] UpdateQuantityRequest updateQuantityRequest, CancellationToken cancellationToken = default);
}