using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Request.OrderProduct;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class OrderProductMapping
{
    public static CreateOrderProductSQLRequest MapToCreateOrderProductRequest(
        this OrderProduct orderProduct, long orderId, double unitPrice, double subtotal)
    {
        return new CreateOrderProductSQLRequest
        {
            OrderId = orderId,
            ProductId = orderProduct.ProductId,
            Quantity = orderProduct.Quantity,
            UnitPrice = unitPrice,
            Subtotal = subtotal
        };
    }

    public static UpdateOrderProductSQLRequest MapToUpdateOrderProductRequest(
        this OrderProduct updateOrderProductRequest, double subtotal, long orderId)
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
        this List<OrderProduct> createOrderProductRequests,
        List<FullProductSQLResponse> productSqlResponse, 
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
        this List<OrderProduct> updateOrderProductRequests,
        List<FullProductSQLResponse> productSqlResponse, 
        long orderId)
    {
        return updateOrderProductRequests
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