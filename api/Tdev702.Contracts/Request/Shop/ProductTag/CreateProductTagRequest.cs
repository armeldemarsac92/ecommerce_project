namespace Tdev702.Contracts.Request.Shop.ProductTag;

public class CreateProductTagRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}