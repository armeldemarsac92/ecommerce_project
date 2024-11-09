using Tdev702.Contracts.SQL.Request.Shop;
using Tdev702.Contracts.SQL.Request.Shop.Product;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductRepository
{
    public Task<ProductResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public ProductRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<ProductResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<ProductResponse>(ProductQueries.GetProductById, new { ProductId = id }, cancellationToken);
    }

    public async Task<List<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<ProductResponse>(ProductQueries.GetAllProducts, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductResponse>();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<ProductResponse>(ProductQueries.CreateProduct, request, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.ExecuteAsync(ProductQueries.UpdateProduct, request, cancellationToken);
    }
}