namespace Tdev702.Contracts.SQL.Request.Shop.ProductTag;

public class UpdateTagRequest
{
    public long ProductTagId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}