namespace Tdev702.Repository.SQL;

public static class OrderQueries
{
    public static string GetOrderById = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE id = @OrderId;";
    
    public static string GetOrderBySessionId = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE stripe_session_id = @StripeSessionId;";

    public static string GetAllOrdersByDateRange = @"
   SELECT *
   FROM backoffice.vw_orders_summary ord
   WHERE ord.updated_at BETWEEN @StartDate AND @EndDate;";

    public static string GetAllOrders = @"
   SELECT *
   FROM backoffice.vw_orders_summary        
   ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";
    
    public static string GetAllOrdersByUserId = @"
   SELECT *
   FROM backoffice.vw_orders_summary
   WHERE user_id = @UserId        
   ORDER BY 
        CASE WHEN @orderBy = 2 THEN id END DESC,
        CASE WHEN @orderBy = 1 THEN id END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

    public static string CreateOrder = @"
   INSERT INTO backoffice.orders (
       user_id, 
       updated_at, 
       created_at, 
       stripe_payment_status,
       stripe_session_status,
       total_amount)
   VALUES (
       @UserId, 
       CURRENT_TIMESTAMP, 
       CURRENT_TIMESTAMP, 
       @StripePaymentStatus::backoffice.payment_status,
       @StripeSessionStatus::backoffice.session_status,
       @TotalAmount)
   RETURNING id;";

    public static string UpdateOrder = @"
   UPDATE backoffice.orders
   SET 
       user_id = COALESCE(@UserId, user_id),
       stripe_invoice_id = COALESCE(@StripeInvoiceId, stripe_invoice_id),
       updated_at = CURRENT_TIMESTAMP,
       stripe_session_id = COALESCE(@StripeSessionId, stripe_session_id),
       stripe_payment_status = COALESCE(@StripePaymentStatus::backoffice.payment_status, stripe_payment_status),
       stripe_session_status = COALESCE(@StripeSessionStatus::backoffice.session_status, stripe_session_status),
       total_amount = COALESCE(@TotalAmount, total_amount)
   WHERE id = @Id;";

    public static string DeleteOrder = @"
   DELETE FROM backoffice.orders
   WHERE id = @OrderId;";
}