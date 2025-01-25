using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Brand;

public class UpdateBrandRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}