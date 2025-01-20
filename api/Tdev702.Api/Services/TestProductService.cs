using Tdev702.Contracts.Request.Shop.Product;
using Tdev702.Contracts.Response.Shop;

namespace Tdev702.Api.Services;

public class TestProductService : IProductsService
{
    public async Task<ShopProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return new ShopProductResponse()
        {
            Id = 1,
            Title = "Test Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<List<ShopProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return new List<ShopProductResponse>()
        {
            new ShopProductResponse()
            {
                Id = 1,
                Title = "Test Product 2",
                Price = 20.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProductResponse()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProductResponse()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProductResponse()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            }
        };
    }

    public async Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        return new ShopProductResponse()
        {
            Id = 1,
            Title = "Created Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ShopProductResponse> UpdateAsync(long id, UpdateProductRequest updateProductRequest,
        CancellationToken cancellationToken = default)
    {
        return new ShopProductResponse()
        {
            Id = 1,
            Title = "Updated Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }
}