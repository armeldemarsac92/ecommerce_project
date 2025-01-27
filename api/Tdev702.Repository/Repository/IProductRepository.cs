using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductRepository
{
    public Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateProductSQLRequest request, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductSQLRequest request, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    private readonly IDbContext _dbContext;

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<ProductSQLResponse>(ProductQueries.GetProductById, new { Id = id }, cancellationToken);
    }

    public async Task<List<ProductSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<ProductSQLResponse>(ProductQueries.GetAllProducts, queryOptions, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }
    
    public async Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<ProductSQLResponse>(
            ProductQueries.GetProductsByIds, 
            new { ProductIds = productIds }, 
            cancellationToken);
    
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }

    public async Task<int> CreateAsync(CreateProductSQLRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(ProductQueries.CreateProduct, request, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateProductSQLRequest request, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(ProductQueries.UpdateProduct, request, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbContext.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id }, cancellationToken);
    }
}