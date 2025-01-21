using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Category;

public class UpdateCategoryRequest
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}