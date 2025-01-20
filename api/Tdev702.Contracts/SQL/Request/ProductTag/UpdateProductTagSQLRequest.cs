namespace Tdev702.Contracts.SQL.Request.ProductTag;

public class UpdateProductTagSQLRequest
{
    public long ProductTagId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}