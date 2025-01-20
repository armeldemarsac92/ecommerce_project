namespace Tdev702.Contracts.API.Request.ProductTag;

public class CreateProductTagRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}