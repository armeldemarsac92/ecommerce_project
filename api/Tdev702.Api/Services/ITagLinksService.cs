using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.ProductTagLink;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ITagLinksService
{
    public Task<TagLinks> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<TagLinks>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<TagLinks> CreateAsync(CreateTagLinksRequest createTagLinksRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class TagLinksService : ITagLinksService
{
    private readonly ITagLinkRepository _tagLinkRepository;
    
    public TagLinksService(ITagLinkRepository tagLinkRepository )
    {
        _tagLinkRepository = tagLinkRepository;
    }

    public async Task<TagLinks> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _tagLinkRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"TagLinks {id} not found");
        
        return response.MapToTagLink();
    }

    public async Task<List<TagLinks>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _tagLinkRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToTagLinks() : throw new NotFoundException($"TagLinks not found");
    }

    public async Task<TagLinks> CreateAsync(CreateTagLinksRequest createTagLinksRequest,
        CancellationToken cancellationToken = default)
    {
        var response = await _tagLinkRepository.CreateAsync(createTagLinksRequest, cancellationToken);
        return response.MapToTagLink();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _tagLinkRepository.DeleteAsync(id, cancellationToken);
    }
}