using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Shop;

public class Product
{
    [JsonPropertyName("id")]
    public required long Id { get; init; }
    
    [JsonPropertyName("stripe_id")]
    public string? StripeId { get; init; }
    
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    [JsonPropertyName("price")]
    public required double Price { get; init; }
    
    [JsonPropertyName("price_ht")]
    public double PriceHt { get; init; }
    
    [JsonPropertyName("brand_id")]
    public long? BrandId { get; init; }
    
    [JsonPropertyName("tags_id")]
    public long? TagsId { get; init; }
    
    [JsonPropertyName("category_id")]
    public long? CategoryId { get; init; }
    
    [JsonPropertyName("open_food_fact_id")]
    public long? OpenFoodFactId { get; init; }
    
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }
    
    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
}