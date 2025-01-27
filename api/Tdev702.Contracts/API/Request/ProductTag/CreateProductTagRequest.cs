using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.ProductTag;

public class CreateProductTagRequest
{
    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }
    
    [JsonPropertyName("tag_id")]
    public required long TagId { get; set; }
}