using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response.Shop;

public class TagLinksResponse
{
    [Column("id")]
    public required long Id { get; init; }
    [Column("product_id")]
    public required long ProductId { get; init; }
    [Column("tag_id")]
    public required long TagId { get; init; }
}