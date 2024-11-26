using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Shop;

public class TagLinks
{
    [JsonPropertyName("id")]
    public required long Id { get; init; }
    
    [JsonPropertyName("tag_id")]
    public long TagId { get; init; }
    
    [JsonPropertyName("product_id")]
    public long ProductId { get; init; }
}