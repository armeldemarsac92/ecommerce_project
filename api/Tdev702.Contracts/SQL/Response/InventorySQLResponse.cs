using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response.Shop;

public class InventorySQLResponse
{
    [Column("Id")]
    public required long Id { get; init; }
    
    [Column("Product_id")]
    public required long ProductId { get; init; }
    
    [Column("Quantity")]
    public required long Quantity { get; init; }
    
    [Column("Sku")]
    public required string Sku { get; init; }
    
    [Column("Created_at")]
    public required DateTimeOffset  CreatedAt { get; init; }
    
    [Column("Update_at")]
    public DateTimeOffset  UpdatedAt { get; init; }
    
}