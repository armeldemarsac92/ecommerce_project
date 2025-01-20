namespace Tdev702.Contracts.API.Shop;

public class Inventory
{
    public required long Id { get; init; }
    public required long ProductId { get; init; }
    public required long Quantity { get; init; }
    public string Sku { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}