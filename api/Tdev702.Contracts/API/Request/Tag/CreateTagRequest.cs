using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Tag;

public class CreateTagRequest
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}