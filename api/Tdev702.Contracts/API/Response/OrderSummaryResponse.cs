using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class OrderSummaryResponse
{
    [JsonPropertyName("order_id")]
    public required int Id { get; init; }

    [JsonPropertyName("user_id")] 
    public required string UserId { get; init; }

    [JsonPropertyName("stripe_payment_status")]
    public required string StripePaymentStatus { get; init; }    
    
    [JsonPropertyName("stripe_session_status")]
    public required string StripeSessionStatus { get; init; }

    [JsonPropertyName("total_amount")]
    public required double TotalAmount { get; init; }

    [JsonPropertyName("stripe_invoice_id")]
    public string? StripeInvoiceId { get; init; }

    [JsonPropertyName("stripe_payment_intent_id")]
    public string? StripeSessionId { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("order_items")]
    public OrderItemResponse[]? OrderItems { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; init; }
}

public class OrderItemResponse
{
    [JsonPropertyName("product_id")]
    public long? ProductId { get; init; }
    
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    [JsonPropertyName("picture")]
    public string? ImageUrl { get; init; }
    
    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }
    
    [JsonPropertyName("unit_price")]
    public double? UnitPrice { get; init; }
    
    [JsonPropertyName("subtotal")]
    public double? Subtotal { get; init; }
    
    [JsonPropertyName("brand")]
    public string? Brand { get; init; }
    
    [JsonPropertyName("category")]
    public string? Category { get; init; }
}