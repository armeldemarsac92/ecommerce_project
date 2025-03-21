using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface IOrderEndpoints
{
    [Get(ShopRoutes.Orders.GetById)]
    Task<ApiResponse<OrderSummaryResponse>> GetByIdAsync(long orderId, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Orders.GetUserOrders)]
    Task<ApiResponse<List<OrderSummaryResponse>>> GetUserOrdersAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Orders.GetAll)]
    Task<ApiResponse<List<OrderSummaryResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Post(ShopRoutes.Orders.Create)]
    Task<ApiResponse<OrderSummaryResponse>> CreateAsync([Body] CreateOrderRequest createOrderRequest, CancellationToken cancellationToken = default);
    
    [Put(ShopRoutes.Orders.Update)]
    Task<ApiResponse<OrderSummaryResponse>> UpdateAsync(long orderId, [Body] UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Orders.GetInvoice)]
    Task<ApiResponse<object>> GetInvoiceAsync(long orderId, CancellationToken cancellationToken = default);
}