using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Order;

public class UpdateOrderRequest
{
    [JsonPropertyName("products" )]
    public required List<OrderProduct> Products { get; set; }
}