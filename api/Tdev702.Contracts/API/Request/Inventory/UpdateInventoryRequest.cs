using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Inventory;

public class UpdateInventoryRequest
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("quantity")]
    public long? Quantity { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("sku")]
    public string? Sku { get; set; }
}