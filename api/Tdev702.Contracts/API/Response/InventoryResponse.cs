namespace Tdev702.Contracts.API.Response;

public class InventoryResponse
{
    public required long Id { get; init; }
    public required long ProductId { get; init; }
    public required long Quantity { get; init; }
    public required string Sku { get; init; }
    public required DateTimeOffset  CreatedAt { get; init; }
    public DateTimeOffset  UpdatedAt { get; init; }
}