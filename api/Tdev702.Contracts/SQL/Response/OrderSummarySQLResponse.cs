using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Response;

public class OrderSummarySQLResponse
{
    [Column("id")]
    public required int Id { get; init; }

    [Column("user_id")] 
    public required string UserId { get; init; }

    [Column("stripe_payment_status")]
    public required string StripePaymentStatus { get; init; }   
    
    [Column("stripe_session_status")]
    public required string StripeSessionStatus { get; init; }

    [Column("total_amount")]
    public required double TotalAmount { get; init; }

    [Column("stripe_invoice_id")]
    public string? StripeInvoiceId { get; init; }

    [Column("stripe_session_id")]
    public string? StripeSessionId { get; init; }

    [Column("created_at")]
    public DateTime CreatedAt { get; init; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; }

    [Column("order_items")]
    public OrderItem[]? OrderItems { get; init; }
}

public class OrderItem
{
    [JsonPropertyName("product_id")]
    public int? ProductId { get; init; }
    
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    [JsonPropertyName("picture")]
    public string? Picture { get; init; }
    
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