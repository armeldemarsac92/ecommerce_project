using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Brand;
using Tdev702.Contracts.SQL.Request.Shop.ProductTag;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ITagsService
{
    public Task<Tag> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Tag> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<Tag> UpdateAsync(long id, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
public class TagsService : ITagsService
{
    private readonly ITagRepository _tagRepository;

    public TagsService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
        
    }
    
    public async Task<Tag> GetByIdAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetByIdAsync(productTagId, cancellationToken);
        if(response is null) throw new NotFoundException($"Product Tags {productTagId} not found");
        
        return response.MapToTag();
    }

    public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToTags() :  throw new NotFoundException("Product Tags not found");
    }

    public async Task<Tag> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.CreateAsync(createTagRequest, cancellationToken);
        return response.MapToTag();
    }

    public async Task<Tag> UpdateAsync(long productTagId, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default)
    {
        updateTagRequest.ProductTagId = productTagId;
        var affectedRows = await _tagRepository.UpdateAsync(updateTagRequest, cancellationToken);

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