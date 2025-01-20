using Tdev702.Contracts.Response.Shop;
using Tdev702.Contracts.SQL;

namespace Tdev702.Contracts.Mapping;

public static class ProductMapping
{
    public static ShopProductResponse MapToProduct(this ProductResponse productResponse)
    {
        return new ShopProductResponse
        {
            Id = productResponse.Id,
            StripeId = productResponse.StripeId,
            Title = productResponse.Title,
            Description = productResponse.Description,
            Price = productResponse.Price,
            PriceHt = productResponse.Price / 1.2,
            BrandId = productResponse.BrandId,
            CategoryId = productResponse.CategoryId,
            OpenFoodFactId = productResponse.OpenFoodFactId,
            UpdatedAt = productResponse.UpdatedAt,
            CreatedAt = productResponse.CreatedAt
        };
    }

    public static List<ShopProductResponse> MapToProducts(this List<ProductResponse> productResponses)
    {
        return productResponses.Select(MapToProduct).ToList();
    }
}