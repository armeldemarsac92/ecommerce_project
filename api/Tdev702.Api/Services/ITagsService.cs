using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ITagsService
{
    public Task<TagResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<TagResponse> UpdateAsync(long id, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
public class TagsService : ITagsService
{
    private readonly ITagRepository _tagRepository;

    public TagsService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
        
    }
    
    public async Task<TagResponse> GetByIdAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetByIdAsync(productTagId, cancellationToken);
        if(response is null) throw new NotFoundException($"Product Tags {productTagId} not found");
        
        return response.MapToTag();
    }

    public async Task<List<TagResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToTags() :  throw new NotFoundException("Product Tags not found");
    }

    public async Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        var sqlRequest = createTagRequest.MapToCreateTagRequest();
        var response = await _tagRepository.CreateAsync(sqlRequest, cancellationToken);
        return response.MapToTag();
    }

    public async Task<TagResponse> UpdateAsync(long productTagId, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default)
    {
        var sqlRequest = updateTagRequest.MapToUpdateTagRequest(productTagId);
        var affectedRows = await _tagRepository.UpdateAsync(sqlRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product Tag {productTagId} not found");
        
        var updatedProductTag = await _tagRepository.GetByIdAsync(productTagId, cancellationToken);
        return updatedProductTag.MapToTag();
        
    }

    public async Task DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var affectedRows = await _tagRepository.DeleteAsync(productTagId, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Product Tag {productTagId} not found");
    }
}