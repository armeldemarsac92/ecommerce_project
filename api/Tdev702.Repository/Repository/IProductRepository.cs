using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductRepository
{
    public Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default);
    public Task<ProductSQLResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public ProductRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<ProductSQLResponse>(ProductQueries.GetProductById, new { Id = id }, cancellationToken);
    }

    public async Task<List<ProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<ProductSQLResponse>(ProductQueries.GetAllProducts, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }
    
    public async Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<ProductSQLResponse>(
            ProductQueries.GetProductsByIds, 
            new { ProductIds = productIds }, 
            cancellationToken);
    
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }

    public async Task<ProductSQLResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<ProductSQLResponse>(ProductQueries.CreateProduct, request, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.ExecuteAsync(ProductQueries.UpdateProduct, request, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id }, cancellationToken);
    }
}