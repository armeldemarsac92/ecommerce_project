namespace Tdev702.Contracts.SQL.Request.Shop.Inventory;

public class CreateInventoryRequest
{
    public required long ProductId { get; set; }

    public required long Quantity { get; set; }
    
    public string Sku { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public required DateTimeOffset? CreatedAt { get; set; }
}