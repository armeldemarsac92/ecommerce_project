namespace Tdev702.Contracts.SQL.Request.Shop.Category;

public class UpdateCategoryRequest
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}