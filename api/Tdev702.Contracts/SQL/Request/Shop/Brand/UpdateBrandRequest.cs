namespace Tdev702.Contracts.SQL.Request.Shop.Brand;

public class UpdateBrandRequest
{
    public long BrandId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}