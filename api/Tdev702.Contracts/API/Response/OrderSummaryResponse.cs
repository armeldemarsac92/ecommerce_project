using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.API.Response;

public class OrderSummaryResponse
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
    public OrderItemResponse[]? OrderItems { get; init; }
}

public class OrderItemResponse
{
    public int? ProductId { get; init; }
    public string? Title { get; init; }
    public int? Quantity { get; init; }
    public double? UnitPrice { get; init; }
    public double? Subtotal { get; init; }
    public string? Brand { get; init; }
    public string? Category { get; init; }
}