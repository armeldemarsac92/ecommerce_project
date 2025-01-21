namespace Tdev702.Repository.SQL;

public static class OrderProductQueries
{
    public static string GetOrderProductById = @"
   SELECT *
   FROM backoffice.link_orders_products
   WHERE id = @OrderProductId;";

    public static string GetAllOrderProducts = @"
   SELECT *
   FROM backoffice.link_orders_products;";
    
    public static string GetAllOrderProductsByOrderId = @"
   SELECT *
   FROM backoffice.link_orders_products
   WHERE order_id = @OrderId;";

    public static string CreateOrderProduct = @"
   INSERT INTO backoffice.link_orders_products (
       product_id,
       order_id,
       quantity,
       unit_price,
       subtotal)
   VALUES (
       @ProductId,
       @OrderId,
       @Quantity,
       @UnitPrice,
       @Subtotal)
   RETURNING *;";
    
    public static string CreateManyOrderProducts = @"
    INSERT INTO backoffice.link_orders_products (
        product_id,
        order_id,
        quantity,
        unit_price,
        subtotal)
    SELECT 
        unnest(@ProductIds),
        unnest(@OrderIds),
        unnest(@Quantities),
        unnest(@UnitPrices),
        unnest(@Subtotals)
    RETURNING *;";
    
    public static string UpdateManyOrderProducts = @"
    UPDATE backoffice.link_orders_products AS op
    SET 
        product_id = COALESCE(u.product_id, op.product_id),
        order_id = COALESCE(u.order_id, op.order_id),
        unit_price = COALESCE(u.unit_price, op.unit_price),
        quantity = COALESCE(u.quantity, op.quantity),
        subtotal = COALESCE(u.subtotal, op.subtotal)
    FROM (
        SELECT 
            unnest(@ProductIds) as product_id,
            unnest(@OrderIds) as order_id,
            unnest(@Quantities) as quantity,
            unnest(@UnitPrices) as unit_price,
            unnest(@Subtotals) as subtotal
    ) AS u
    WHERE op.order_id = u.order_id AND op.product_id = u.product_id
    RETURNING *;";

    public static string UpdateOrderProduct = @"
   UPDATE backoffice.link_orders_products
   SET 
       product_id = @ProductId,
       order_id = @OrderId,
       quantity = @Quantity,
       subtotal = @Subtotal
   WHERE id = @OrderProductId;";

    public static string DeleteOrderProduct = @"
   DELETE FROM backoffice.link_orders_products
   WHERE id = @OrderProductId;";
}