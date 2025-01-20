using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Response.Shop;

public class BrandResponse
{
    [JsonPropertyName("id")]
    public long BrandId { get; init; }
    
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}