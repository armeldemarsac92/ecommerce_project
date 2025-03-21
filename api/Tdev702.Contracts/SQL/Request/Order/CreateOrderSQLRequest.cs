namespace Tdev702.Contracts.SQL.Request.Order;

public class CreateOrderSQLRequest
{
    public string UserId { get; set; }
    public string? StripePaymentStatus { get; set; }
    public string? StripeSessionStatus { get; set; }
    public double TotalAmount { get; set; }
}