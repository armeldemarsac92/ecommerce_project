using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductTagRepository
{
    public Task<ProductTagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductTagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductTagSQLResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductTagRequest updateProductTagRequest, CancellationToken cancellationToken = default);
    public Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class ProductTagRepository : IProductTagRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public ProductTagRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }
    
    public async Task<ProductTagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<ProductTagSQLResponse>(ProductTagQueries.GetProductTagById, new { ProductTagId = id }, cancellationToken);
    }

    public async Task<List<ProductTagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<ProductTagSQLResponse>(ProductTagQueries.GetAllProductTags, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductTagSQLResponse>();
    }

    public async Task<ProductTagSQLResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<ProductTagSQLResponse>(ProductTagQueries.CreateProductTag, createProductTagRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateProductTagRequest updateProductTagRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbCommand.ExecuteAsync(ProductTagQueries.UpdateProductTag, updateProductTagRequest, cancellationToken);
    }

    public async Task<int> DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.ExecuteAsync(ProductTagQueries.DeleteProductTag, new { ProductTagId = productTagId}, cancellationToken);
    }
}