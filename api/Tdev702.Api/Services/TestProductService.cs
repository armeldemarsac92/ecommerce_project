using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.SQL.Request.Shop;

namespace Tdev702.Api.Services;

public class TestProductService : IProductsService
{
    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return new Product()
        {
            Id = 1,
            Title = "Test Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return new List<Product>()
        {
            new Product()
            {
                Id = 1,
                Title = "Test Product 2",
                Price = 20.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new Product()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new Product()
            {
                Id = 1,
                Title = "Test Product",
                Price = 10.99,
                Description = "This is a test product",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new Product()
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

    public async Task<Product> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        return new Product()
        {
            Id = 1,
            Title = "Created Product",
            Price = 10.99,
            Description = "This is a test product",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public async Task<Product> UpdateAsync(long productId, UpdateProductRequest updateProductRequest,
        CancellationToken cancellationToken = default)
    {
        return new Product()
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