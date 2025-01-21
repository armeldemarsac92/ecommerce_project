namespace Tdev702.Contracts.API.Request.Brand;

public class CreateBrandRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}