using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Request.Shop.Product;

namespace Tdev702.Api.Services;

public class TestProductService : IProductsService
{
    public async Task<ShopProduct> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return new ShopProduct()
        {
            Id = 1,
            Title = "Test Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<List<ShopProduct>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return new List<ShopProduct>()
        {
            new ShopProduct()
            {
                Id = 1,
                Title = "Test Product 2",
                Price = 20.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProduct()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProduct()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new ShopProduct()
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

    public async Task<ShopProduct> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        return new ShopProduct()
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

    public async Task<ShopProduct> UpdateAsync(long id, UpdateProductRequest updateProductRequest,
        CancellationToken cancellationToken = default)
    {
        return new ShopProduct()
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