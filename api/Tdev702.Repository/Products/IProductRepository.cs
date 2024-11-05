using Tdev702.Contracts.SQL.Request.Shop;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Repository.Products;

public interface IProductRepository
{
    public Task<ProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    public async Task<ProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}