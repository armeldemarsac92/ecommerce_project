namespace Tdev702.Contracts.SQL.Request.Shop.Category;

public class CreateCategoryRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}