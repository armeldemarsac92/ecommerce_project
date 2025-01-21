using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Order;

public class UpdateOrderRequest
{
    [JsonPropertyName("stripe_invoice_id" )]
    public string? StripeInvoiceId { get; set; }
    
    [JsonPropertyName("payment_status" )]
    public required string PaymentStatus { get; set; }
    
    [JsonPropertyName("products" )]
    public List<UpdateOrderProductRequest> Products { get; set; }
}