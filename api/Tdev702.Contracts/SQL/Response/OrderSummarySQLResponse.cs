using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Response;

public class OrderSummarySQLResponse
{
    [Column("id")]
    public required int Id { get; init; }

    [Column("user_id")] 
    public required string UserId { get; init; }

    [Column("payment_status")]
    public required string PaymentStatus { get; init; }

    [Column("total_amount")]
    public required double TotalAmount { get; init; }

    [Column("stripe_invoice_id")]
    public string? StripeInvoiceId { get; init; }

    [Column("stripe_payment_intent_id")]
    public string? StripePaymentIntentId { get; init; }

    [Column("created_at")]
    public required DateTime CreatedAt { get; init; }

    [Column("order_items")]
    public OrderItem[]? OrderItems { get; init; }
}

public class OrderItem
{
    [JsonPropertyName("product_id")]
    public int? ProductId { get; init; }
    
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
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