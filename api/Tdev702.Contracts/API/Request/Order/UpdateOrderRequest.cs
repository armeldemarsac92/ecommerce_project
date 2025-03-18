using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Order;

public class UpdateOrderRequest
{
    [JsonPropertyName("products" )]
    public List<UpdateOrderProductRequest> Products { get; set; }
}