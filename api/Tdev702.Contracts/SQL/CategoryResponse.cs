using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL;

public class CategoryResponse
{
    [Column("id")]
    public required long Id { get; init; }
    
    [Column("title")]
    public required string Title { get; init; }
    
    [Column("description")]
    public string? Description { get; init; }
}