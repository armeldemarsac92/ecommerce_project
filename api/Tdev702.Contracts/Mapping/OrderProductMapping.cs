using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Request.OrderProduct;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class OrderProductMapping
{
    public static OrderProductResponse MapToOrderProduct(this OrderProductSQLResponse orderProductSqlResponse,
        ProductSQLResponse productSqlResponse)
    {
        return new OrderProductResponse
        {
            Id = orderProductSqlResponse.Id,
            OrderId = orderProductSqlResponse.OrderId,
            Product = productSqlResponse.MapToProduct(),
            Quantity = orderProductSqlResponse.Quantity,
            UnitPrice = orderProductSqlResponse.UnitPrice,
            Subtotal = orderProductSqlResponse.Subtotal
        };
    }

    public static List<OrderProductResponse> MapToOrderProducts(this List<OrderProductSQLResponse> orderProductResponses,
        List<ProductSQLResponse> productResponses)
    {
        return orderProductResponses
            .Where(orderProduct => productResponses.Any(p => p.Id == orderProduct.ProductId))
            .Select(orderProduct => {
                var product = productResponses.First(p => p.Id == orderProduct.ProductId);
                return orderProduct.MapToOrderProduct(product);
            })
            .ToList();
    }

    public static CreateOrderProductSQLRequest MapToCreateOrderProductRequest(
        this CreateOrderProductRequest createOrderProductRequest, long orderId, double unitPrice, double subtotal)
    {
        return new CreateOrderProductSQLRequest
        {
            OrderId = orderId,
            ProductId = createOrderProductRequest.ProductId,
            Quantity = createOrderProductRequest.Quantity,
            UnitPrice = unitPrice,
            Subtotal = subtotal
        };
    }

    public static UpdateOrderProductSQLRequest MapToUpdateOrderProductRequest(
        this UpdateOrderProductRequest updateOrderProductRequest, double subtotal, long orderId)
    {
        return new UpdateOrderProductSQLRequest
        {
            OrderId = orderId,
            ProductId = updateOrderProductRequest.ProductId,
            Quantity = updateOrderProductRequest.Quantity,
            Subtotal = subtotal
        };
    }
    
    public static List<CreateOrderProductSQLRequest> MapToCreateOrderProductRequests(
        this List<CreateOrderProductRequest> createOrderProductRequests,
        List<ProductSQLResponse> productSqlResponse, 
        long orderId)
    {
        return createOrderProductRequests
            .Where(orderProduct => productSqlResponse.Any(p => p.Id == orderProduct.ProductId))
            .Select(orderProduct =>
            {
                var product = productSqlResponse.First(p => p.Id == orderProduct.ProductId);

                return orderProduct.MapToCreateOrderProductRequest(
                    orderId,
                    product.Price,
                    product.Price * orderProduct.Quantity
                );
            })
            .ToList();
    }
    
    public static List<UpdateOrderProductSQLRequest> MapToUpdateOrderProductRequests(
        this List<UpdateOrderProductRequest> updateOrderProductRequests,
        List<ProductSQLResponse> productSqlResponse, 
        long orderId)
    {
        return updateOrderProductRequests
            .Where(orderProduct => productSqlResponse.Any(p => p.Id == orderProduct.ProductId))
            .Select(orderProduct =>
            {
                var product = productSqlResponse.First(p => p.Id == orderProduct.ProductId);

                return orderProduct.MapToUpdateOrderProductRequest(
                    product.Price * orderProduct.Quantity,
                    orderId
                );
            })
           .ToList( );
    }
    
}