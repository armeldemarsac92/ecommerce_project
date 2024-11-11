namespace Tdev702.Contracts.SQL.Request.Shop.Brand;

public class CreateBrandRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}