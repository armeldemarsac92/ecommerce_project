namespace Tdev702.Contracts.SQL.Request.Category;

public class UpdateCategorySQLRequest
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}