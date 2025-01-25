namespace Tdev702.Contracts.SQL.Request.Tag;

public class UpdateTagSQLRequest
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}