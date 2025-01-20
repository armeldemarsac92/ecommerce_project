namespace Tdev702.Repository.SQL;

public static class OrderQueries
{
    public static string GetOrderById = @"
   SELECT *
   FROM backoffice.orders
   WHERE id = @OrderId;";

    public static string GetAllOrders = @"
   SELECT *
   FROM backoffice.orders;";
    
    public static string GetAllOrdersByUserId = @"
   SELECT *
   FROM backoffice.orders
   WHERE user_id = @UserId;";

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
   RETURNING *;";

    public static string UpdateOrder = @"
   UPDATE backoffice.orders
   SET 
       user_id = COALESCE(@UserId, user_id),
       stripe_invoice_id = COALESCE(@StripeInvoiceId, stripe_invoice_id),
       updated_at = CURRENT_TIMESTAMP,
       payment_status = COALESCE(@PaymentStatus, payment_status),
       total_amount = COALESCE(@TotalAmount, total_amount)
   WHERE id = @OrderId;";

    public static string DeleteOrder = @"
   DELETE FROM backoffice.orders
   WHERE id = @OrderId;";
}