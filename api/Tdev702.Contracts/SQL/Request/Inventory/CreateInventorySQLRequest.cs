namespace Tdev702.Contracts.SQL.Request.Inventory;

public class CreateInventorySQLRequest
{
    public required long ProductId { get; set; }

    public required long Quantity { get; set; }
    
    public string Sku { get; set; }
}