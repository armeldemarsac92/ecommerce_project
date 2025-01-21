using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.ProductTag;

public class CreateProductTagRequest
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}