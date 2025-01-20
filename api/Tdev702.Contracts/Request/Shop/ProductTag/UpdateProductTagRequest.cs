namespace Tdev702.Contracts.Request.Shop.ProductTag;

public class UpdateProductTagRequest
{
    public long ProductTagId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}