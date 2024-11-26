using Tdev702.Contracts.SQL.Request.Shop.ProductTagLink;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ITagLinkRepository
{
    public Task<TagLinksResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagLinksResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TagLinksResponse> CreateAsync(CreateTagLinksRequest createTagLinksRequest, CancellationToken cancellationToken = default);
    
    // public Task<TagLinksResponse?> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    // public Task<TagLinksResponse?> GetByTagIdAsync(long tagId, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class TagTagLinkRepository : ITagLinkRepository
{
    private readonly IDBSQLCommand _dbCommand;
    
    public TagTagLinkRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<TagLinksResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TagLinksResponse>(LinkProductTagQueries.GetLinkById ,new {Id = id}, cancellationToken);
    }

    public async Task<List<TagLinksResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<TagLinksResponse>(LinkProductTagQueries.GetAllLinks, cancellationToken);
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
    
}