using Tdev702.Contracts.SQL.Response;
using ProductTagResponse = Tdev702.Contracts.API.Response.ProductTagResponse;

namespace Tdev702.Contracts.Mapping;

public static class ProductTagMapping
{
    public static ProductTagResponse MapToProductTag(this ProductTagSQLResponse productTagSqlResponse)
    {
        return new ProductTagResponse()
        {
            ProductTagId = productTagSqlResponse.ProductTagId,
            Title = productTagSqlResponse.Title,
            Description = productTagSqlResponse.Description,
        };
    }

    public static List<ProductTagResponse> MapToProductTags(this List<ProductTagSQLResponse> productTagResponses)
    {
        return productTagResponses.Select(MapToProductTag).ToList();
    }
}