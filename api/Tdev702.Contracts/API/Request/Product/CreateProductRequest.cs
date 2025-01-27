using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Product;

public class CreateProductRequest
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }
   
    [JsonPropertyName("description")]
    public string? Description { get; set; }
   
    [JsonPropertyName("price")]
    public required double Price { get; set; }
   
    [JsonPropertyName("brand_id")]
    public long? BrandId { get; set; }
   
    [JsonPropertyName("category_id")]
    public long? CategoryId { get; set; }
   
    [JsonPropertyName("open_food_fact_id")]
    public long? OpenFoodFactId { get; set; }
    
    [JsonPropertyName("tag_ids")]
    public List<long>? TagIds { get; set; }
    
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }
}