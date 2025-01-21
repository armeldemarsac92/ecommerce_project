using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Category;

public class CreateCategoryRequest
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}