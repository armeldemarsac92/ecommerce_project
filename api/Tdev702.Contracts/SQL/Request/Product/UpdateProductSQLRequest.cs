namespace Tdev702.Contracts.SQL.Request.Product;

public class UpdateProductSQLRequest
{
    public long Id { get; set; }
    public string? StripeId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public double? Price { get; init; }
    public long? BrandId { get; init; }
    public long? CategoryId { get; init; }
    public long? OpenFoodFactId { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
}