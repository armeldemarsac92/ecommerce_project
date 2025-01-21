using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class OrderProductResponse
{
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("product")]
    public required ShopProductResponse Product { get; set; }

    [JsonPropertyName("order_id")]
    public required long OrderId { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
    
    [JsonPropertyName("unit_price")]
    public required double UnitPrice { get; set; }

    [JsonPropertyName("subtotal")]
    public required double Subtotal { get; set; }
}