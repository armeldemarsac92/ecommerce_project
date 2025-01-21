using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class ProductTagResponse
{
    [JsonPropertyName("id")]
    public long ProductTagId { get; init; }
    
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}