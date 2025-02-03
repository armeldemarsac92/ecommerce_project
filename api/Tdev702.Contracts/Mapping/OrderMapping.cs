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
    
    
    public static OrderSummaryResponse MapToOrderSummary(this OrderSummarySQLResponse orderSql)
    {
        return new OrderSummaryResponse
        {
            Id = orderSql.Id,
            UserId = orderSql.UserId,
            PaymentStatus = orderSql.PaymentStatus,
            TotalAmount = orderSql.TotalAmount,
            StripeInvoiceId = orderSql.StripeInvoiceId,
            StripePaymentIntentId = orderSql.StripePaymentIntentId,
            CreatedAt = orderSql.CreatedAt,
            OrderItems = orderSql.OrderItems.Select(item => new OrderItemResponse
            {
                ProductId = item.ProductId,
                Title = item.Title,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Subtotal = item.Subtotal,
                Brand = item.Brand,
                Category = item.Category
            }).ToArray()
        };
    }

    public static List<OrderSummaryResponse> MapToOrderSummaries(this List<OrderSummarySQLResponse> orders)
    {
        return orders.Select(MapToOrderSummary).ToList();
    }


    public static CreateOrderSQLRequest MapToCreateOrderRequest(this CreateOrderRequest createOrderRequest, double totalAmount)
    {
        return new CreateOrderSQLRequest
        {
            UserId = createOrderRequest.UserId,
            TotalAmount = totalAmount,
            
            StripeInvoiceId = createOrderRequest.StripeInvoiceId
        };
    }

    public static UpdateOrderSQLRequest MapToUpdateOrderRequest(this UpdateOrderRequest updateOrderRequest, long orderId, double? totalAmount)
    {
        return new UpdateOrderSQLRequest
        {
            Id = orderId,
            PaymentStatus = updateOrderRequest.PaymentStatus,
            TotalAmount = totalAmount, //the total amount should be calculated based on the updated products, not be passed as a parameter, hence the syntax
            StripeInvoiceId = updateOrderRequest.StripeInvoiceId
        };
    }
    
}