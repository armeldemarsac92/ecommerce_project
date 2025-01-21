namespace Tdev702.Contracts.SQL.Request.Order;

public class CreateOrderSQLRequest
{
    public required string UserId { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public required string PaymentStatus { get; set; }
    public double TotalAmount { get; set; }
}