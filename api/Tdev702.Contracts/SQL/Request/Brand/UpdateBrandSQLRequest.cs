namespace Tdev702.Contracts.SQL.Request.Brand;

public class UpdateBrandSQLRequest
{
    public long BrandId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}