using Tdev702.Contracts.SQL.Request.ProductTag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductTagRepository
{
    public Task<ProductTagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductTagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<ProductTagSQLResponse>> GetAllByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<List<ProductTagSQLResponse>> GetAllByTagIdAsync(long tagId, CancellationToken cancellationToken = default);
    public Task<ProductTagSQLResponse> CreateAsync(CreateProductTagSQLRequest createProductTagRequest, CancellationToken cancellationToken = default);
    public Task<int> DeleteByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<int> DeleteByTagIdAsync(long tagId, CancellationToken cancellationToken = default);

}

public class ProductTagRepository : IProductTagRepository
{
    private readonly IDbContext _dbContext;

    public ProductTagRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProductTagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<ProductTagSQLResponse>(ProductTagQueries.GetProductTagById, new { ProductTagId = id }, cancellationToken);
    }

    public async Task<List<ProductTagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<ProductTagSQLResponse>(ProductTagQueries.GetAllProductTags, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductTagSQLResponse>();
    }

    public async Task<List<ProductTagSQLResponse>> GetAllByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<ProductTagSQLResponse>(ProductTagQueries.GetAllProductTagsByProductId, new { ProductId = productId }, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductTagSQLResponse>();
    }

    public async Task<List<ProductTagSQLResponse>> GetAllByTagIdAsync(long tagId, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<ProductTagSQLResponse>(ProductTagQueries.GetAllProductTagsByTagId, new { TagId = tagId }, cancellationToken);
        return response.Any()? response.ToList() : new List<ProductTagSQLResponse>();
    }
    public async Task<ProductTagSQLResponse> CreateAsync(CreateProductTagSQLRequest createProductTagRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<ProductTagSQLResponse>(ProductTagQueries.CreateProductTag, createProductTagRequest, cancellationToken);
    }

    public async Task<int> DeleteByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(ProductTagQueries.DeleteProductTagByProductId, new { ProductId = productId}, cancellationToken);
    }    
    
    public async Task<int> DeleteByTagIdAsync(long tagId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(ProductTagQueries.DeleteProductTagByTagId, new { TagId = tagId}, cancellationToken);
    }
}