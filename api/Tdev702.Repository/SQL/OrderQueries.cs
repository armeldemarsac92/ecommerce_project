namespace Tdev702.Repository.SQL;

public static class OrderQueries
{
    public static string GetOrderById = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE id = @OrderId;";
    
    public static string GetOrderByIntentId = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE stripe_payment_intent_id = @StripePaymentIntentId;";

    public static string GetAllOrders = @"
   SELECT *
   FROM backoffice.vw_orders_summary        
   ORDER BY 
        CASE WHEN @orderBy = 'DESC' THEN id END DESC,
        CASE WHEN @orderBy = 'ASC' THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";
    
    public static string GetAllOrdersByUserId = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE user_id = @UserId        
   ORDER BY 
        CASE WHEN @orderBy = 'DESC' THEN id END DESC,
        CASE WHEN @orderBy = 'ASC' THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

    public static string CreateOrder = @"
   INSERT INTO backoffice.orders (
       user_id, 
       stripe_invoice_id, 
       updated_at, 
       created_at, 
       stripe_payment_intent_id,
       payment_status,
       total_amount)
   VALUES (
       @UserId, 
       @StripeInvoiceId, 
       CURRENT_TIMESTAMP, 
       CURRENT_TIMESTAMP, 
       @StripePaymentIntentId, 
       @PaymentStatus,
       @TotalAmount)
   RETURNING id;";

    public static string UpdateOrder = @"
   UPDATE backoffice.orders
   SET 
       user_id = COALESCE(@UserId, user_id),
       stripe_invoice_id = COALESCE(@StripeInvoiceId, stripe_invoice_id),
       updated_at = CURRENT_TIMESTAMP,
       stripe_payment_intent_id = COALESCE(@StripePaymentIntentId, stripe_payment_intent_id),
       payment_status = COALESCE(@PaymentStatus, payment_status),
       total_amount = COALESCE(@TotalAmount, total_amount)
   WHERE id = @OrderId;";

    public static string DeleteOrder = @"
   DELETE FROM backoffice.orders
   WHERE id = @OrderId;";
}