using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class ProductTagMapping
{
    public static ProductTag MapToProductTag(this ProductTagResponse productTagResponse)
    {
        return new ProductTag()
        {
            ProductTagId = productTagResponse.ProductTagId,
            Title = productTagResponse.Title,
            Description = productTagResponse.Description,
        };
    }

    public static List<ProductTag> MapToProductTags(this List<ProductTagResponse> productTagResponses)
    {
        return productTagResponses.Select(MapToProductTag).ToList();
    }
}