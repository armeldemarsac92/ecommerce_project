using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class ProductTagSQLResponse
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("product_id")]
    public long ProductId { get; set; }
    
    [Column("tag_id")]
    public long TagId { get; set; }
}