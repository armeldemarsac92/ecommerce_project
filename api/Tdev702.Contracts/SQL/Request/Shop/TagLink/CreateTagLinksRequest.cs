namespace Tdev702.Contracts.SQL.Request.Shop.ProductTagLink;

public class CreateTagLinksRequest
{
    public required long TagId { get; set; }
    public required long ProductId { get; set; }
}