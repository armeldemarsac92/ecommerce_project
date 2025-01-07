using Tdev702.Contracts.SQL.Request.Shop.ProductTag;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ITagRepository
{
    public Task<TagResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class TagRepository : ITagRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public TagRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }
    
    public async Task<TagResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TagResponse>(TagQueries.GetProductTagById, new { ProductTagId = id }, cancellationToken);
    }

    public async Task<List<TagResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagResponse>(TagQueries.GetAllProductTags, cancellationToken);
        return response.Any() ? response.ToList() : new List<TagResponse>();
    }

    public async Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<TagResponse>(TagQueries.CreateProductTag, createTagRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbCommand.ExecuteAsync(TagQueries.UpdateProductTag, updateTagRequest, cancellationToken);
    }

    public async Task<int> DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.ExecuteAsync(TagQueries.DeleteProductTag, new { ProductTagId = productTagId}, cancellationToken);
    }
}