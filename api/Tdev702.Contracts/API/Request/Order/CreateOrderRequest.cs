using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Order;

public class CreateOrderRequest
{
    [JsonPropertyName("user_id" )]
    public required string UserId { get; set; }
    
    [JsonPropertyName("stripe_invoice_id" )]
    public string? StripeInvoiceId { get; set; }
    
    [JsonPropertyName("products" )]
    public List<CreateOrderProductRequest> Products { get; set; }
    
    
    
}
