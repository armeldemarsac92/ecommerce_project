using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class ProductMapping
{
    public static ShopProductResponse MapToProduct(this ProductSQLResponse productSqlResponse)
    {
        return new ShopProductResponse
        {
            Id = productSqlResponse.Id,
            StripeId = productSqlResponse.StripeId,
            Title = productSqlResponse.Title,
            Description = productSqlResponse.Description,
            Price = productSqlResponse.Price,
            PriceHt = productSqlResponse.Price / 1.2,
            BrandId = productSqlResponse.BrandId,
            CategoryId = productSqlResponse.CategoryId,
            OpenFoodFactId = productSqlResponse.OpenFoodFactId,
            UpdatedAt = productSqlResponse.UpdatedAt,
            CreatedAt = productSqlResponse.CreatedAt
        };
    }

    public static List<ShopProductResponse> MapToProducts(this List<ProductSQLResponse> productResponses)
    {
        return productResponses.Select(MapToProduct).ToList();
    }

    public static CreateProductSQLRequest MapToCreateProductRequest(this CreateProductRequest createProductRequest)
    {
        return new CreateProductSQLRequest
        {
            Title = createProductRequest.Title,
            Description = createProductRequest.Description,
            Price = createProductRequest.Price,
            BrandId = createProductRequest.BrandId,
            CategoryId = createProductRequest.CategoryId,
            OpenFoodFactId = createProductRequest.OpenFoodFactId
        };
    }

    public static UpdateProductSQLRequest MapToUpdateProductRequest(this UpdateProductRequest updateProductRequest,
        long productId)
    {
        return new UpdateProductSQLRequest
        {
            Id = productId,
            Title = updateProductRequest.Title,
            Description = updateProductRequest.Description,
            Price = updateProductRequest.Price,
            BrandId = updateProductRequest.BrandId,
            CategoryId = updateProductRequest.CategoryId,
            OpenFoodFactId = updateProductRequest.OpenFoodFactId
        };
    }
}