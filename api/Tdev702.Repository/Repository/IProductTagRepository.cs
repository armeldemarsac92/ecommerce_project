using Tdev702.Contracts.SQL.Request.Shop.ProductTag;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductTagRepository
{
    public Task<ProductTagResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductTagResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductTagResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default);
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
    
    public async Task<ProductTagResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<ProductTagResponse>(ProductTagQueries.GetProductTagById, new { ProductTagId = id }, cancellationToken);
    }

    public async Task<List<ProductTagResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<ProductTagResponse>(ProductTagQueries.GetAllProductTags, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductTagResponse>();
    }

    public async Task<ProductTagResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<ProductTagResponse>(ProductTagQueries.CreateProductTag, createProductTagRequest, cancellationToken);
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