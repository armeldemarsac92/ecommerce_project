using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class InventoryResponse
{
    [JsonPropertyName("id")]
    public required long Id { get; init; }
    
    [JsonPropertyName("product_id")]
    public required long ProductId { get; init; }
    
    [JsonPropertyName("quantity")]
    public required long Quantity { get; init; }
    
    [JsonPropertyName("sku")]
    public required string Sku { get; init; }
    
    [JsonPropertyName("created_at")]
    public required DateTimeOffset  CreatedAt { get; init; }
    
    [JsonPropertyName("updated_at")]
    public DateTimeOffset  UpdatedAt { get; init; }
}