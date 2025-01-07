using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response.Shop;

public class ProductResponse
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
    
    [Column("tag_id")]
    public long? TagId { get; init; }
    
    [Column("brand_id")]
    public long? BrandId { get; init; }
    
    [Column("category_id")]
    public long? CategoryId { get; init; }
    
    [Column("open_food_fact_id")]
    public long? OpenFoodFactId { get; init; }
    
    [Column("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }
    
    [Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }
    
    
}