using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IProductsService
{
    public Task<ShopProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ShopProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> UpdateAsync(long id, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<IProductsService> _logger;

    public ProductsService(IProductRepository productRepository, ILogger<IProductsService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }


    public async Task<ShopProductResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _productRepository.GetByIdAsync(id, cancellationToken);
            if(response is null) throw new NotFoundException($"Product {id} not found");
        
            return response.MapToProduct();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<ShopProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProducts() : throw new NotFoundException("No products found");
    }

    public async Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.CreateAsync(createProductRequest, cancellationToken);
        return response.MapToProduct();
    }

    public async Task<ShopProductResponse> UpdateAsync(long Id, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default)
    {
        updateProductRequest.Id = Id;
        var affectedRows = await _productRepository.UpdateAsync(updateProductRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product {Id} not found");
        
        var updatedProduct = await _productRepository.GetByIdAsync(Id, cancellationToken);
        return updatedProduct.MapToProduct();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _productRepository.DeleteAsync(id, cancellationToken);
    }
}