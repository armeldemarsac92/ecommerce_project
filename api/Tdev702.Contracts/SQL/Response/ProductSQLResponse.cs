using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class ProductSQLResponse
{
    [Column("id")]
    public required int Id { get; init; }

    [Column("title")]
    public required string Title { get; init; }

    [Column("description")]
    public required string Description { get; init; }

    [Column("price")] 
    public required double Price { get; init; }

    [Column("image_url")]
    public required string ImageUrl { get; init; }

    [Column("tags")]
    public string[]? Tags { get; init; }

    [Column("brand_title")]
    public string? BrandTitle { get; init; }

    [Column("category_title")]
    public string? CategoryTitle { get; init; }

    [Column("open_food_fact_id")]
    public string? OpenFoodFactId { get; init; }

    [Column("updated_at")]
    public required DateTime UpdatedAt { get; init; }

    [Column("created_at")]
    public required DateTime CreatedAt { get; init; }
}