using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Product;
using Tdev702.Contracts.SQL.Request.Shop.ProductTagLink;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IProductsService
{
    public Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Product> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    public Task<Product> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    public Task<Product> GetProductByIdAsync(long productId, CancellationToken cancellationToken = default);
    
}

public class ProductsService : IProductsService
{
    private readonly ITagLinkRepository _tagLinkRepository;
    private readonly IProductRepository _productRepository;
    private readonly ITagRepository _tagRepository;

    public ProductsService(IProductRepository productRepository, ITagLinkRepository tagLinkRepository, ITagRepository tagRepository)
    {
        _productRepository = productRepository;
        _tagLinkRepository = tagLinkRepository;
        _tagRepository = tagRepository;
    }
    

    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Product {id} not found");
        
        var tags = await _tagLinkRepository.GetTagByProductIdAsync(id, cancellationToken);
        return response.MapToProduct(tags);
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.GetAllAsync(cancellationToken);
        var tags = await _tagLinkRepository.GetAllByProductIdAsync(cancellationToken);
        return response.Any() ? response.MapToProducts(tags) : throw new NotFoundException("No products found");
    }

    public async Task<Product> CreateAsync(CreateProductRequest createProductRequest,CancellationToken cancellationToken = default)
    {
        foreach(var tagid in createProductRequest.TagIds)
        {
            var tag = await _tagRepository.GetByIdAsync(tagid, cancellationToken);
            if (tag == null) throw new BadRequestException("Tag could not be found");
        }
        
        var createdProduct = await _productRepository.CreateAsync(createProductRequest, cancellationToken);
        foreach (var tag in createProductRequest.TagIds)
        {
            await _tagLinkRepository.CreateAsync(new CreateTagLinksRequest()
            {
                ProductId = createdProduct.Id,
                TagId = tag,
            });
        }
        var createdTags = await _tagLinkRepository.GetTagByProductIdAsync(createdProduct.Id, cancellationToken);
        return createdProduct.MapToProduct();
    }

    public async Task<Product> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default)
    {
        updateProductRequest.Id = productId;
        var affectedRows = await _productRepository.UpdateAsync(updateProductRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product {productId} not found");
        
        var updatedProduct = await _productRepository.GetByIdAsync(productId, cancellationToken);
        var tags = await _tagLinkRepository.GetTagByProductIdAsync(updatedProduct.Id, cancellationToken);
        return updatedProduct.MapToProduct(tags);
        
        
    }
}