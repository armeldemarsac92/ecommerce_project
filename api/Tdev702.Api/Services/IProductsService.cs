using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Product;
using Tdev702.Repository.Products;

namespace Tdev702.Api.Services;

public interface IProductsService
{
    public Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Product> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    public Task<Product> UpdateAsync(long id, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;

    public ProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Product {id} not found");
        
        return response.MapToProduct();
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProducts() : throw new NotFoundException("No products found");
    }

    public async Task<Product> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        var response = await _productRepository.CreateAsync(createProductRequest, cancellationToken);
        return response.MapToProduct();
    }

    public async Task<Product> UpdateAsync(long Id, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default)
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