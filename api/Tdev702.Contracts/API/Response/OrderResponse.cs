using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class OrderResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("user_id")]
    public required string UserId { get; set; }

    [JsonPropertyName("stripe_invoice_id")]
    public string? StripeInvoiceId { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("stripe_payment_intent_id")]
    public string? StripePaymentIntentId { get; set; }

    [JsonPropertyName("payment_status")]
    public required string PaymentStatus { get; set; }
    
    [JsonPropertyName("total_amount")]
    public double TotalAmount { get; set; }
    
    [JsonPropertyName("products")]
    public required List<OrderProductResponse> Products { get; set; }
}