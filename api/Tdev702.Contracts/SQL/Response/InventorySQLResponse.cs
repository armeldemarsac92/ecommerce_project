using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class InventorySQLResponse
{
    [Column("id")]
    public required long Id { get; init; }
    
    [Column("product_id")]
    public required long ProductId { get; init; }
    
    [Column("quantity")]
    public required long Quantity { get; init; }
    
    [Column("sku")]
    public required string Sku { get; init; }
    
    [Column("created_at")]
    public DateTimeOffset  CreatedAt { get; init; }
    
    [Column("update_at")]
    public DateTimeOffset  UpdatedAt { get; init; }
    
}