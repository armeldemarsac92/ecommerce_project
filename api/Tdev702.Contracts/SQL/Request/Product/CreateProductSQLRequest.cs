namespace Tdev702.Contracts.SQL.Request.Product;

public class CreateProductSQLRequest
{
    public string? StripeId { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public required double Price { get; set; }
    
    public long? BrandId { get; set; }
    
    public long? CategoryId { get; set; }
    
    public long? OpenFoodFactId { get; set; }
    
    public string? ImageUrl { get; set; }
    
}