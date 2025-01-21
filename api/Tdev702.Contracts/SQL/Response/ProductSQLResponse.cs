using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class ProductSQLResponse
{
    [Column("id")]
    public required long Id { get; init; }
    
    [Column("stripe_id")]
    public string? StripeId { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("description")]
    public string? Description { get; init; }
    
    [Column("price")]
    public required double Price { get; init; }
    
    [Column("brand_id")]
    public long? BrandId { get; init; }
    
    [Column("category_id")]
    public long? CategoryId { get; init; }
    
    [Column("open_food_fact_id")]
    public long? OpenFoodFactId { get; init; }
    
    [Column("image_url")]
    public string? ImageUrl { get; set; }
    
    [Column("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }
    
    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
    
    
}