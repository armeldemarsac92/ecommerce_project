using Tdev702.Contracts.SQL.Request.All;

namespace Tdev702.Contracts.Mapping;

public static class QueryOptionsMapping
{
    public static QueryOptions MapToQueryOptions(string? pageSize, string? pageNumber, string? sortBy)
    {
        return new QueryOptions
        {
            PageSize = int.TryParse(pageSize, out int size) ? size : 30,
            PageNumber = int.TryParse(pageNumber, out int page) ? page : 1,
            OrderBy = Enum.TryParse<QueryOptions.Order>(sortBy, true, out var order) ? order : QueryOptions.Order.ASC
        };
    }
}