using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response.Shop;

public class ProductTagResponse
{
    [Column("id")]
    public required long ProductTagId { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("description")]
    public string? Description { get; init; }
}