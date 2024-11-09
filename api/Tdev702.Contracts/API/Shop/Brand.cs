using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Shop;

public class Brand
{
    [JsonPropertyName("id")]
    public long Id { get; init; }
    
    [JsonPropertyName("title")]
    public string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}