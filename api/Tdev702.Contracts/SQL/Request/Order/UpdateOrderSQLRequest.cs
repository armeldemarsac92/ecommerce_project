using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Request.Order;

public class UpdateOrderSQLRequest
{
    public required long Id { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? PaymentStatus { get; set; }
    public double TotalAmount { get; set; }
}