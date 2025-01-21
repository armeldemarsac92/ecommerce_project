using System.Text.Json.Serialization;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Contracts.API.Request.Order;

public class CreateOrderProductRequest
{
    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
}