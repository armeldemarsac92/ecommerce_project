using Tdev702.Contracts.SQL.Request.Shop.ProductTagLink;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ITagLinkRepository
{
    public Task<TagLinksResponse?> GetTagByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<TagLinksResponse?> GetByTagIdAsync(long tagId, CancellationToken cancellationToken = default);
    public Task<List<TagLinksResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<TagLinksResponse>> GetAllByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<List<TagLinksResponse>> GetAllByTagIdAsync(long tagId, CancellationToken cancellationToken = default);
    public Task<List<TagLinksResponse>> GetAllByTagIdAndProductIdAsync(long tagId, long productId, CancellationToken cancellationToken = default);
    public Task<TagLinksResponse> CreateAsync(CreateTagLinksRequest createTagLinksRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    public Task DeleteByTagIdAsync(long tagId, CancellationToken cancellationToken = default);
    public Task DeleteByProductIdAsync(long productId, CancellationToken cancellationToken = default);
}

public class TagLinkRepository : ITagLinkRepository
{
    private readonly IDBSQLCommand _dbCommand;
    
    public TagLinkRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }
    public async Task<TagLinksResponse?> GetTagByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TagLinksResponse>(LinkProductTagQueries.GetTagByProductId, new {ProductId = productId}, cancellationToken);
    }
    
    public async Task<TagLinksResponse?> GetByTagIdAsync(long tagId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TagLinksResponse>(LinkProductTagQueries.GetLinkByTagId, new {TagId = tagId}, cancellationToken);
    }
    public async Task<List<TagLinksResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagLinksResponse>(LinkProductTagQueries.GetAllLinks, cancellationToken);
        return response.Any() ? response.ToList() : new List<TagLinksResponse>();
    }
    public async Task<List<TagLinksResponse>> GetAllByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagLinksResponse>(LinkProductTagQueries.GetTagByProductId, new {ProductId = productId}, cancellationToken);
        return response.Any() ? response.ToList() : new List<TagLinksResponse>();
    }

    public async Task<List<TagLinksResponse>> GetAllByTagIdAsync(long tagId, CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagLinksResponse>(LinkProductTagQueries.GetLinkByTagId, new {TagId = tagId} , cancellationToken);
        return response.Any() ? response.ToList() : new List<TagLinksResponse>();
    }
    
    public async Task<List<TagLinksResponse>> GetAllByTagIdAndProductIdAsync(long tagId, long productId, CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagLinksResponse>(LinkProductTagQueries.GetLinkByTagAndProductId, new {TagId = tagId, ProductId = productId} , cancellationToken);
        return response.Any() ? response.ToList() : new List<TagLinksResponse>();
    }
    public async Task<TagLinksResponse> CreateAsync(CreateTagLinksRequest createTagLinksRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<TagLinksResponse>(LinkProductTagQueries.CreateLinkProductTag, createTagLinksRequest, cancellationToken);
    }
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(LinkProductTagQueries.DeleteLinkProductTag, new {Id = id}, cancellationToken);
    }
    public async Task DeleteByTagIdAsync(long tagId, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(LinkProductTagQueries.DeleteLinkByTagId, new {TagId = tagId}, cancellationToken);
    }
    public async Task DeleteByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(LinkProductTagQueries.DeleteLinkByProductId, new {ProductId = productId}, cancellationToken);
    }
}