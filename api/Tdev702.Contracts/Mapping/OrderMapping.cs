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
            StripePaymentStatus = orderSql.StripePaymentStatus,
            StripeSessionStatus = orderSql.StripeSessionStatus,
            TotalAmount = orderSql.TotalAmount,
            StripeInvoiceId = orderSql.StripeInvoiceId,
            StripeSessionId = orderSql.StripeSessionId,
            UpdatedAt = orderSql.UpdatedAt,
            CreatedAt = orderSql.CreatedAt,
            OrderItems = orderSql.OrderItems?
                .Where(item => item.ProductId != null)
                .Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId!.Value,
                    Title = item.Title,
                    Quantity = item.Quantity!.Value,
                    Description = item.Description,
                    ImageUrl = item.Picture,
                    UnitPrice = item.UnitPrice!.Value,
                    Subtotal = item.Subtotal!.Value,
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
            TotalAmount = totalAmount,
        };
    }

    public static UpdateOrderSQLRequest MapToUpdateOrderRequest(this UpdateOrderRequest updateOrderRequest, long orderId, double? totalAmount)
    {
        return new UpdateOrderSQLRequest
        {
            Id = orderId,
            TotalAmount = totalAmount, //the total amount should be calculated based on the updated products, not be passed as a parameter, hence the syntax
        };
    }
    
}