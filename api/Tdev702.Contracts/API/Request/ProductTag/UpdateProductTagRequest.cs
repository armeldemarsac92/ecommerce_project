using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.ProductTag;

public class UpdateProductTagRequest
{
    [JsonPropertyName("product_tag_id")]
    public long ProductTagId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}