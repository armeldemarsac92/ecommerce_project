using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.Tag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ITagRepository
{
    public Task<TagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagSQLResponse>> GetByIdsAsync(List<long> tagIds, CancellationToken cancellationToken = default);
    public Task<List<TagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TagSQLResponse> CreateAsync(CreateTagSQLRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateTagSQLRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class TagRepository : ITagRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public TagRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<TagSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.QueryFirstOrDefaultAsync<TagSQLResponse>(TagQueries.GetTagById, new { TagId = id }, cancellationToken);
    }

    public async Task<List<TagSQLResponse>> GetByIdsAsync(List<long> tagIds, CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryAsync<TagSQLResponse>(TagQueries.GetByIds, new { TagIds = tagIds, cancellationToken});
        return response.Any() ? response.ToList() : new List<TagSQLResponse>();
    }

    public async Task<List<TagSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryAsync<TagSQLResponse>(TagQueries.GetAllTags, cancellationToken);
        return response.Any() ? response.ToList() : new List<TagSQLResponse>();
    }

    public async Task<TagSQLResponse> CreateAsync(CreateTagSQLRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.QuerySingleAsync<TagSQLResponse>(TagQueries.CreateTag, createTagRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateTagSQLRequest updateTagRequest, CancellationToken cancellationToken = default)
    { 
        return await _unitOfWork.ExecuteAsync(TagQueries.UpdateTag, updateTagRequest, cancellationToken);
    }

    public async Task<int> DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteAsync(TagQueries.DeleteTag, new { TagId = productTagId}, cancellationToken);
    }
}