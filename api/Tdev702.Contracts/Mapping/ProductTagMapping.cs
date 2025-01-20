using ProductTagResponse = Tdev702.Contracts.Response.Shop.ProductTagResponse;

namespace Tdev702.Contracts.Mapping;

public static class ProductTagMapping
{
    public static ProductTagResponse MapToProductTag(this SQL.ProductTagResponse productTagResponse)
    {
        return new ProductTagResponse()
        {
            ProductTagId = productTagResponse.ProductTagId,
            Title = productTagResponse.Title,
            Description = productTagResponse.Description,
        };
    }

    public static List<ProductTagResponse> MapToProductTags(this List<SQL.ProductTagResponse> productTagResponses)
    {
        return productTagResponses.Select(MapToProductTag).ToList();
    }
}