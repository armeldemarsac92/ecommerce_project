using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Brand;

public class CreateBrandRequest
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}