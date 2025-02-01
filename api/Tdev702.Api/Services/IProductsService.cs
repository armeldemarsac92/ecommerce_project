using MassTransit;
using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Request.ProductTag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Request.ProductTag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;
using Tdev702.Repository.SQL;

namespace Tdev702.Api.Services;

public interface IProductsService
{
    public Task<ShopProductResponse> GetByIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<List<ShopProductResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default);
    public Task<ShopProductResponse> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default);
    public Task AddPictureAsync(long productId, string imageUrl, CancellationToken cancellationToken = default);
    public Task CreateNutrimentsAsync(CreateNutrimentSQLRequest createNutrimentRequest, CancellationToken cancellationToken = default);
    public Task UpdateNutrimentsAsync(UpdateNutrimentSQLRequest updateNutrimentSqlRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long productId ,CancellationToken cancellationToken = default);
}

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductTagRepository _productTagRepository;
    private readonly INutrimentsRepository _nutrimentsRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IProductRepository productRepository, 
        IProductTagRepository productTagRepository, 
        ILogger<ProductsService> logger, 
        ITagRepository tagRepository, 
        IUnitOfWork unitOfWork, 
        INutrimentsRepository nutrimentsRepository, 
        IPublishEndpoint publishEndpoint)
    {
        _productRepository = productRepository;
        _productTagRepository = productTagRepository;
        _logger = logger;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _nutrimentsRepository = nutrimentsRepository;
        _publishEndpoint = publishEndpoint;
    }


    public async Task<ShopProductResponse> GetByIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting product with id: {productId}", productId);
        var response = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if(response is null) throw new NotFoundException($"Product {productId} not found");

        await _publishEndpoint.Publish(response, cancellationToken);
        return response.MapToProduct();

    }

    public async Task<List<ShopProductResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all products");
        var response = await _productRepository.GetAllAsync(queryOptions, cancellationToken);
        return response.Any() ? response.MapToProducts() : throw new NotFoundException("No products found");
    }

    public async Task<ShopProductResponse> CreateAsync(CreateProductRequest createProductRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new product {productName}", createProductRequest.Title);
        var sqlRequest = createProductRequest.MapToCreateProductRequest();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            
            var productId = await _productRepository.CreateAsync(sqlRequest, cancellationToken);
            _logger.LogInformation("Product {productId} created successfully.", productId);

            if (createProductRequest.TagIds != null && createProductRequest.TagIds.Any())
            {
                _logger.LogInformation("Creating product tags for product {productId}", productId);
                foreach (var tagId in createProductRequest.TagIds)
                {
                    await _productTagRepository.CreateAsync(
                        new CreateProductTagSQLRequest() { ProductId = productId, TagId = tagId },
                        cancellationToken);
                }

                _logger.LogInformation("Product tags created successfully for product {productId}", productId);
            }
            
            var productResponse = await _productRepository.GetByIdAsync(productId, cancellationToken);
            
            await _unitOfWork.CommitAsync(cancellationToken);
            
            if(productResponse.OpenFoodFactId != null) await _publishEndpoint.Publish(productResponse, cancellationToken);

            return productResponse.MapToProduct();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Failed to create product {name}: {message}", createProductRequest.Title, ex.Message);
            throw;
        }
        
    }

    public async Task<ShopProductResponse> UpdateAsync(long productId, UpdateProductRequest updateProductRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating product {productId}", productId);
        var sqlRequest = updateProductRequest.MapToUpdateProductRequest(productId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var productTags = await _productTagRepository.GetAllByProductIdAsync(productId, cancellationToken);
            if (productTags.Any() && updateProductRequest.TagIds.Any())
            {
                _logger.LogInformation("Deleting removed product tags for product {productId}", productId);
                var productTagsToDelete = productTags.Where(tag => !updateProductRequest.TagIds.Contains(tag.Id));
                foreach (var productTag in productTagsToDelete)
                {
                    await _productTagRepository.DeleteAsync(productTag.Id, cancellationToken);
                }
    
                _logger.LogInformation("Creating new product tags for product {productId}", productId);
                var productTagsToCreate = updateProductRequest.TagIds
                    .Where(tagId => !productTags.Any(pt => pt.Id == tagId))
                    .Select(tagId => new CreateProductTagSQLRequest() 
                    { 
                        ProductId = productId,
                        TagId = tagId 
                    });

                foreach (var newProductTag in productTagsToCreate)
                {
                    await _productTagRepository.CreateAsync(newProductTag, cancellationToken);
                }
            }
            else if (productTags.Any() && !updateProductRequest.TagIds.Any())
            {
                _logger.LogInformation("Deleting all product tags for product {productId}", productId);
                foreach (var productTag in productTags)
                {
                    await _productTagRepository.DeleteAsync(productTag.Id, cancellationToken);
                }
            }

            var affectedRows = await _productRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Product {productId} not found");
        
            var updatedProduct = await _productRepository.GetByIdAsync(productId, cancellationToken);
            _logger.LogInformation("Product {productId} updated successfully.", productId);
            
            await _unitOfWork.CommitAsync(cancellationToken);
            return updatedProduct.MapToProduct();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Failed to update product {productId}: {message}.", productId, ex.Message);
            throw;
        }
        
    }

    public async Task AddPictureAsync(long productId, string imageUrl, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding picture to product {productId}", productId);
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _productRepository.UpdateAsync(new UpdateProductSQLRequest(){ImageUrl = imageUrl, Id = productId}, cancellationToken);
            _logger.LogInformation("Picture added to product {productId} successfully.", productId);
            
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to add picture to product {productId}: {message}", productId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task CreateNutrimentsAsync(CreateNutrimentSQLRequest createNutrimentRequest, CancellationToken cancellationToken = default)
    {
       _logger.LogInformation("Creating OpenFoodFact data for product {productId}", createNutrimentRequest.ProductId);
       
       await _unitOfWork.BeginTransactionAsync(cancellationToken);
       try
       {
           await _nutrimentsRepository.CreateAsync(createNutrimentRequest, cancellationToken);
           _logger.LogInformation("OpenFoodFact data created successfully for product {productId}",
               createNutrimentRequest.ProductId);
           await _unitOfWork.CommitAsync(cancellationToken);
       }
       catch (Exception ex)
       {
           _logger.LogError("Failed to create OpenFoodFact data for product {productId}: {message}", createNutrimentRequest.ProductId, ex.Message);
           await _unitOfWork.RollbackAsync(cancellationToken);
           throw;
       }
    }

    public async Task UpdateNutrimentsAsync(UpdateNutrimentSQLRequest updateNutrimentSqlRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating OpenFoodFact data for product {productId}", updateNutrimentSqlRequest.ProductId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _nutrimentsRepository.UpdateAsync(updateNutrimentSqlRequest, cancellationToken);
            _logger.LogInformation("OpenFoodFact data updated successfully for product {productId}",
                updateNutrimentSqlRequest.ProductId);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Failed to update OpenFoodFact data for product {productId}: {message}",
                updateNutrimentSqlRequest.ProductId, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(long productId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting product {productId}", productId);
        await _productRepository.DeleteAsync(productId, cancellationToken);
        await _productTagRepository.DeleteByProductIdAsync(productId, cancellationToken);
        _logger.LogInformation("Product {productId} deleted successfully.", productId);
    }
}