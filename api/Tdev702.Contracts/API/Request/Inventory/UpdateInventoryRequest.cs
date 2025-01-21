using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Inventory;

public class UpdateInventoryRequest
{
    [JsonPropertyName("quantity")]
    public long? Quantity { get; set; }

    [JsonPropertyName("sku")]
    public string? Sku { get; set; }
}