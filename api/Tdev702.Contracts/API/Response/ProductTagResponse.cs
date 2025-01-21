using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class ProductTagResponse
{
    [JsonPropertyName("id" )]
    public required long Id { get; set; }
    
    [JsonPropertyName("product_id")]
    public required long ProductId { get; set; }
    
    [JsonPropertyName("tag_id")]
    public required long TagId { get; set; }
}