using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class ShopProductResponse
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("price")] 
    public required double Price { get; init; }

    [JsonPropertyName("price_ht")] 
    public required double PriceHt { get; init; }

    [JsonPropertyName("image_url")]
    public required string ImageUrl { get; init; }

    [JsonPropertyName("tags")]
    public string[]? Tags { get; init; }

    [JsonPropertyName("brand_title")]
    public string? BrandTitle { get; init; }

    [JsonPropertyName("category_title")]
    public string? CategoryTitle { get; init; }

    [JsonPropertyName("open_food_fact_id")]
    public string? OpenFoodFactId { get; init; }

    [JsonPropertyName("updated_at")]
    public required DateTime UpdatedAt { get; init; }

    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; init; }
}