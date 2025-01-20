namespace Tdev702.Contracts.SQL.Request.Shop.Inventory;

public class UpdateInventoryRequest
{
    public long Id { get; set; }
    public long? ProductId { get; set; }
    public long? Quantity { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? Sku { get; set; }
}