namespace Tdev702.Contracts.SQL.Request.Shop.Brand;

public class UpdateBrandRequest
{
    public required long Id { get; set; }
    public string? Title { get; set; }
    public string? Desription { get; set; }
}