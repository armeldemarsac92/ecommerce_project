using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Request.OrderProduct;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class OrderMapping
{
    public static OrderResponse MapToOrder(this OrderSQLResponse orderSqlResponse, List<OrderProductResponse> productSqlResponse)
    {
        return new OrderResponse
        {
            Id = orderSqlResponse.Id,
            UserId = orderSqlResponse.UserId,
            PaymentStatus = orderSqlResponse.PaymentStatus,
            TotalAmount = orderSqlResponse.TotalAmount,
            CreatedAt = orderSqlResponse.CreatedAt,
            UpdatedAt = orderSqlResponse.UpdatedAt,
            StripeInvoiceId = orderSqlResponse.StripeInvoiceId,
            Products = productSqlResponse.Where(p => p.OrderId == orderSqlResponse.Id).ToList()
        };
    }
    
    
    public static List<OrderResponse> MapToOrders(this List<OrderSQLResponse> orderResponses, List<OrderProductResponse> productSqlResponse)
    {
        return orderResponses.Select(order => order.MapToOrder(productSqlResponse)).ToList();
    }
    

    public static CreateOrderSQLRequest MapToCreateOrderRequest(this CreateOrderRequest createOrderRequest, double totalAmount)
    {
        return new CreateOrderSQLRequest
        {
            UserId = createOrderRequest.UserId,
            TotalAmount = totalAmount,
            
            PaymentStatus = createOrderRequest.PaymentStatus,
            StripeInvoiceId = createOrderRequest.StripeInvoiceId
        };
    }

    public static UpdateOrderSQLRequest MapToUpdateOrderRequest(this UpdateOrderRequest updateOrderRequest, long orderId, double totalAmount)
    {
        return new UpdateOrderSQLRequest
        {
            Id = orderId,
            PaymentStatus = updateOrderRequest.PaymentStatus,
            TotalAmount = totalAmount,
            StripeInvoiceId = updateOrderRequest.StripeInvoiceId
        };
    }
    
}