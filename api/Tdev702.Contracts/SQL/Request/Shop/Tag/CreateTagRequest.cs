namespace Tdev702.Contracts.SQL.Request.Shop.ProductTag;

public class CreateTagRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}