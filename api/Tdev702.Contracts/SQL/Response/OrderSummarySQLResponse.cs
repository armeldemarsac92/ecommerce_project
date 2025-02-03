using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class OrderSummarySQLResponse
{
    [Column("order_id")]
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
    public required OrderItem[] OrderItems { get; init; }
}

public class OrderItem
{
    [Column("product_id")]
    public required int ProductId { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("quantity")]
    public required int Quantity { get; init; }
    
    [Column("unit_price")]
    public required double UnitPrice { get; init; }
    
    [Column("subtotal")]
    public required double Subtotal { get; init; }
    
    [Column("brand")]
    public string? Brand { get; init; }
    
    [Column("category")]
    public string? Category { get; init; }
}