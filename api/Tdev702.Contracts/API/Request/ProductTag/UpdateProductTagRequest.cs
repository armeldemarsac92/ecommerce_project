namespace Tdev702.Contracts.API.Request.ProductTag;

public class UpdateProductTagRequest
{
    public long ProductTagId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}