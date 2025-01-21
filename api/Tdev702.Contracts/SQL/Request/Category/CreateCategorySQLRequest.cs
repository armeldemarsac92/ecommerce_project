namespace Tdev702.Contracts.SQL.Request.Category;

public class CreateCategorySQLRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}