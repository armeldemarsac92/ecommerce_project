using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Response;

public class OrderSQLResponse
{
    [Column("id")]
    public required long Id { get; set; }
    
    [Column("user_id")]
    public required string UserId { get; set; }

    [Column("stripe_invoice_id")]
    public string? StripeInvoiceId { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("created_at")] 
    public DateTime CreatedAt { get; set; }
    
    [Column("stripe_payment_intent_id")]
    public string? StripePaymentIntentId { get; set; }

    [Column("payment_status")]
    public required string PaymentStatus { get; set; }

    [Column("total_amount")]
    public double TotalAmount { get; set; }
}