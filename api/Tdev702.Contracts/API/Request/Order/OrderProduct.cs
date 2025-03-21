using System.Text.Json.Serialization;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Contracts.API.Request.Order;

public class OrderProduct
{
    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
}