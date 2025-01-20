using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IProductTagsService
{
    public Task<ProductTagResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductTagResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductTagResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default);
    public Task<ProductTagResponse> UpdateAsync(long id, UpdateProductTagRequest updateProductTagRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
public class ProductTagsService : IProductTagsService
{
    private readonly IProductTagRepository _productTagRepository;

    public ProductTagsService(IProductTagRepository productTagRepository)
    {
        _productTagRepository = productTagRepository;
        
    }
    
    public async Task<ProductTagResponse> GetByIdAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var response = await _productTagRepository.GetByIdAsync(productTagId, cancellationToken);
        if(response is null) throw new NotFoundException($"Product Tags {productTagId} not found");
        
        return response.MapToProductTag();
    }

    public async Task<List<ProductTagResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productTagRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProductTags() :  throw new NotFoundException("Product Tags not found");
    }

    public async Task<ProductTagResponse> CreateAsync(CreateProductTagRequest createProductTagRequest, CancellationToken cancellationToken = default)
    {
        var response = await _productTagRepository.CreateAsync(createProductTagRequest, cancellationToken);
        return response.MapToProductTag();
    }

    public async Task<ProductTagResponse> UpdateAsync(long productTagId, UpdateProductTagRequest updateProductTagRequest, CancellationToken cancellationToken = default)
    {
        updateProductTagRequest.ProductTagId = productTagId;
        var affectedRows = await _productTagRepository.UpdateAsync(updateProductTagRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product Tag {productTagId} not found");
        
        var updatedProductTag = await _productTagRepository.GetByIdAsync(productTagId, cancellationToken);
        return updatedProductTag.MapToProductTag();
        
    }

    public async Task DeleteAsync(long productTagId, CancellationToken cancellationToken = default)
    {
        var affectedRows = await _productTagRepository.DeleteAsync(productTagId, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Product Tag {productTagId} not found");
    }
}