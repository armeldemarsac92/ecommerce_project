namespace Tdev702.Contracts.SQL.Request.Order;

public class CreateOrderSQLRequest
{
    public required string UserId { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? StripePaymentStatus { get; set; }
    public string? StripeSessionsStatus { get; set; }
    public double TotalAmount { get; set; }
}