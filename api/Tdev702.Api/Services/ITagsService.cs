using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ITagsService
{
    public Task<TagResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default);
    public Task<TagResponse> UpdateAsync(long tagId, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long tagId, CancellationToken cancellationToken = default);
}
public class TagsService : ITagsService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TagsService> _logger;

    public TagsService(ITagRepository tagRepository, 
        IUnitOfWork unitOfWork,
        ILogger<TagsService> logger)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<TagResponse> GetByIdAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetByIdAsync(productTagId, cancellationToken);
        if(response is null) throw new NotFoundException($"Product Tags {productTagId} not found");
        
        return response.MapToTag();
    }

    public async Task<List<TagResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _tagRepository.GetAllAsync(queryOptions, cancellationToken);
        return response.Any() ? response.MapToTags() :  throw new NotFoundException("Product Tags not found");
    }

    public async Task<TagResponse> CreateAsync(CreateTagRequest createTagRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new tag {tagName}", createTagRequest.Title);
        var sqlRequest = createTagRequest.MapToCreateTagRequest();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var tagId = await _tagRepository.CreateAsync(sqlRequest, cancellationToken);
            _logger.LogInformation("Tag {tagId} created successfully.", tagId);
            var tagResponse = await _tagRepository.GetByIdAsync(tagId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return tagResponse.MapToTag();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating tag {tagName}: {message}", createTagRequest.Title, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<TagResponse> UpdateAsync(long tagId, UpdateTagRequest updateTagRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating tag {tagId}", tagId);
        var sqlRequest = updateTagRequest.MapToUpdateTagRequest(tagId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _tagRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Product Tag {tagId} not found");

            _logger.LogInformation("Tag {tagId} updated successfully.", tagId);
            var tagResponse = await _tagRepository.GetByIdAsync(tagId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return tagResponse.MapToTag();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating tag {tagId}: {message}", tagId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteAsync(long tagId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting tag {tagId}", tagId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _tagRepository.DeleteAsync(tagId, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Product Tag {tagId} not found");
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Tag {tagId} deleted successfully.", tagId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting tag {tagId}: {message}", tagId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
       
    }
}