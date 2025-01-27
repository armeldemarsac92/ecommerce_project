namespace Tdev702.Contracts.SQL.Request.All;

public class QueryOptions
{
    public int PageSize { get; set; } = 30;
    public int PageNumber { get; set; } = 1;
    public Order OrderBy { get; set; }
    public int Offset => (PageNumber - 1) * PageSize; 

    public enum Order
    {
        ASC,
        DESC
    }
}