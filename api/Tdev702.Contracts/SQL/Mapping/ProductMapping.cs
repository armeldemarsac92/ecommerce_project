using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Contracts.SQL.Mapping;

public static class ProductMapping
{
    public static Product MapToProduct(this ProductResponse productResponse, TagResponse? tagResponse)
    {
        return new Product
        {
            Id = productResponse.Id,
            StripeId = productResponse.StripeId,
            Title = productResponse.Title,
            Description = productResponse.Description,
            Price = productResponse.Price,
            TagsId = tagResponse.TagId,
            PriceHt = productResponse.Price / 1.2,
            BrandId = productResponse.BrandId,
            CategoryId = productResponse.CategoryId,
            OpenFoodFactId = productResponse.OpenFoodFactId,
            UpdatedAt = productResponse.UpdatedAt,
            CreatedAt = productResponse.CreatedAt
        };
    }

    public static List<Product> MapToProducts(this List<ProductResponse> productResponses, TagResponse? tagLinksResponses)
    {
        return productResponses.Select(x => MapToProduct(x, tagLinksResponses)).ToList();
    }
}