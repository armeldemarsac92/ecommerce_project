namespace Tdev702.Contracts.SQL.Request.ProductTag;

public class CreateProductTagSQLRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}