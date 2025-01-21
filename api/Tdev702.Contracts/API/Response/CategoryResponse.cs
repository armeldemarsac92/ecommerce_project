using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class CategoryResponse
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
}