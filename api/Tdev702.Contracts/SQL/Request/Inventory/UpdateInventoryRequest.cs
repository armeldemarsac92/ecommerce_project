namespace Tdev702.Contracts.SQL.Request.Inventory;

public class UpdateInventorySQLRequest
{
    public long Id { get; set; }
    public long? Quantity { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? Sku { get; set; }
}