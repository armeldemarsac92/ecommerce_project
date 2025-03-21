using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Order;

public class CreateOrderRequest
{
    [JsonPropertyName("products" )]
    public List<OrderProduct> Products { get; set; }
    
    
    
}
