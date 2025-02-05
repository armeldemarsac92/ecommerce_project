using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Request.Order;

public class UpdateOrderSQLRequest
{
    public required long Id { get; set; }
    public string? UserId { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? StripeSessionStatus { get; set; }
    public string? StripePaymentStatus { get; set; }
    public double? TotalAmount { get; set; }
    public string? StripeSessionId { get; set; }
}