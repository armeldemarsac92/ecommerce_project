using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.ProductTag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IProductsService
{
    public Task<ShopProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ShopProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductTagRepository _productTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IProductRepository productRepository, IProductTagRepository productTagRepository, ILogger<ProductsService> logger, ITagRepository tagRepository)
    {
        _productRepository = productRepository;
        _productTagRepository = productTagRepository;
        _logger = logger;
        _tagRepository = tagRepository;
    }


    public async Task<ShopProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting product with id: {productId}", id);
        var response = await _productRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Product {id} not found");
        
        return await MapToProductResponse(response, cancellationToken);

    }

    public async Task<List<ShopProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all products");
        var response = await _productRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProducts() : throw new NotFoundException("No products found");
    }

    public async Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new product {productName}", createProductRequest.Title);
        var sqlRequest = createProductRequest.MapToCreateProductRequest();
        var productResponse = await _productRepository.CreateAsync(sqlRequest, cancellationToken);
        _logger.LogInformation("Product {productId} created successfully.", productResponse.Id);
        
        if (createProductRequest.TagIds != null && createProductRequest.TagIds.Any())
        {
            _logger.LogInformation("Creating product tags for product {productId}", productResponse.Id);
            foreach (var tagId in createProductRequest.TagIds)
            {
                await _productTagRepository.CreateAsync(new CreateProductTagSQLRequest() { ProductId = productResponse.Id, TagId = tagId }, cancellationToken);
            }
            _logger.LogInformation("Product tags created successfully for product {productId}", productResponse.Id);
        }
        return await MapToProductResponse(productResponse, cancellationToken);
    }

    public async Task<ShopProductResponse> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating product {productId}", productId);
        var sqlRequest = updateProductRequest.MapToUpdateProductRequest(productId);
        var affectedRows = await _productRepository.UpdateAsync(sqlRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product {productId} not found");
        
        var updatedProduct = await _productRepository.GetByIdAsync(productId, cancellationToken);
        _logger.LogInformation("Product {productId} updated successfully.", productId);
        
        return await MapToProductResponse(updatedProduct, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting product {productId}", id);
        await _productRepository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Product {productId} deleted successfully.", id);
    }
    
    private async Task<ShopProductResponse> MapToProductResponse(
        ProductSQLResponse updatedProduct, CancellationToken cancellationToken)
    {
        var productTags = await _productTagRepository.GetAllByProductIdAsync(updatedProduct.Id, cancellationToken);
        var tags = await _tagRepository.GetByIdsAsync(productTags.Select(pt => pt.TagId).ToList(), cancellationToken);
        var mappedTags = tags.MapToTags();
        return updatedProduct.MapToProduct(mappedTags);
    }
}