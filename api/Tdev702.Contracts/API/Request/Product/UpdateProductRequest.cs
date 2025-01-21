using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Product;

public class UpdateProductRequest
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("stripe_id")] 
    public string? StripeId { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("price")]
    public double? Price { get; init; }

    [JsonPropertyName("brand_id")]
    public long? BrandId { get; init; }

    [JsonPropertyName("category_id")]
    public long? CategoryId { get; init; }

    [JsonPropertyName("open_food_fact_id")]
    public long? OpenFoodFactId { get; init; }
    
    [JsonPropertyName("tag_ids")]
    public List<long>? TagIds { get; set; }
}