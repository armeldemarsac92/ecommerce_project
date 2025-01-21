using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class BrandSQLResponse
{
    [Column("id")]
    public required long BrandId { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("description")]
    public string? Description { get; init; }
}