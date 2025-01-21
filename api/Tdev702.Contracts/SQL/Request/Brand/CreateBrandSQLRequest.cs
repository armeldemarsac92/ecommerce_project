namespace Tdev702.Contracts.SQL.Request.Brand;

public class CreateBrandSQLRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}