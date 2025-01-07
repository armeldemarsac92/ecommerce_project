using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response.Shop;

public class TagResponse
{
    [Column("id")]
    public required long TagId { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("description")]
    public string? Description { get; init; }
}