using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Response;

public class OrderProductSQLResponse
{
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }

    [JsonPropertyName("order_id")]
    public required long OrderId { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
    
    [JsonPropertyName("unit_price")]
    public required double UnitPrice { get; set; }

    [JsonPropertyName("subtotal")]
    public required double Subtotal { get; set; }
}