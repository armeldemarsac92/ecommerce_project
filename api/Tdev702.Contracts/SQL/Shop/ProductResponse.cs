namespace Tdev702.Contracts.SQL.Shop;

public class ProductResponse
{
    public required long Id { get; init; }
    public string? StripeId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}