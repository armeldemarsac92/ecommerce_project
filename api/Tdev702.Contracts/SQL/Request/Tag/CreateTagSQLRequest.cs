namespace Tdev702.Contracts.SQL.Request.Tag;

public class CreateTagSQLRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}