namespace Tdev702.Contracts.API.Request.Category;

public class UpdateCategoryRequest
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}