using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Tag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ITagRepository
{
    public Task<TagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagSQLResponse>> GetByIdsAsync(List<long> tagIds, CancellationToken cancellationToken = default);
    public Task<List<TagSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateTagSQLRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateTagSQLRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class TagRepository : ITagRepository
{
    private readonly IDbContext _dbContext;

    public TagRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<TagSQLResponse>(TagQueries.GetTagById, new { TagId = id }, cancellationToken);
    }

    public async Task<List<TagSQLResponse>> GetByIdsAsync(List<long> tagIds, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<TagSQLResponse>(TagQueries.GetByIds, new { TagIds = tagIds, cancellationToken});
        return response.Any() ? response.ToList() : new List<TagSQLResponse>();
    }

    public async Task<List<TagSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<TagSQLResponse>(TagQueries.GetAllTags, queryOptions, cancellationToken);
        return response.Any() ? response.ToList() : new List<TagSQLResponse>();
    }

    public async Task<int> CreateAsync(CreateTagSQLRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(TagQueries.CreateTag, createTagRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateTagSQLRequest updateTagRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbContext.ExecuteAsync(TagQueries.UpdateTag, updateTagRequest, cancellationToken);
    }

    public async Task<int> DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(TagQueries.DeleteTag, new { TagId = productTagId}, cancellationToken);
    }
}