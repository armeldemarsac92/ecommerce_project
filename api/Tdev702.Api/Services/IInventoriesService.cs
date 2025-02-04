using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IInventoriesService
{
    public Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventoryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);  
    public Task<InventoryResponse> IncreamentAsync(int addedQuantity, long productId, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> DecrementAsync(int substractedQuantity ,long productId, CancellationToken cancellationToken = default);
}

public class InventoriesService : IInventoriesService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InventoriesService> _logger;

    public InventoriesService(IInventoryRepository inventoryRepository, 
        IProductRepository productRepository, 
        ILogger<InventoriesService> logger, 
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting inventory with id: {id}", id);
        var response = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Inventory {id} not found");
        
        return response.MapToInventory();
    }

    public async Task<List<InventoryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all inventories");
        var response = await _inventoryRepository.GetAllAsync(queryOptions, cancellationToken);
        return response.Any() ? response.MapToInventories() :  throw new NotFoundException("Inventories not found");
    }

    public async Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting inventory by product id: {productId}", productId);
        var response = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if(response is null) throw new NotFoundException($"The inventory for this product id: {productId} was not found");
        return response.MapToInventory();
    }

    public async Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new inventory for product id: {productId}", createProductInventoryRequest.ProductId);
        var sqlRequest = createProductInventoryRequest.MapToInventoryRequest();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var createdInventoryId = await _inventoryRepository.CreateAsync(sqlRequest, cancellationToken);
            _logger.LogInformation("Inventory {inventoryId} created successfully.", createdInventoryId);
            var inventoryResponse = await _inventoryRepository.GetByIdAsync(createdInventoryId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return inventoryResponse.MapToInventory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create inventory for product id: {productId}: {message}", createProductInventoryRequest.ProductId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<InventoryResponse> UpdateAsync(long inventoryId, UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating inventory with id: {inventoryId}", inventoryId);
        var sqlRequest = updateInventoryRequest.MapToInventoryRequest(inventoryId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Inventory {inventoryId} not found");
            var updatedProduct = await _inventoryRepository.GetByIdAsync(inventoryId, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Inventory {inventoryId} updated successfully.", inventoryId);
            return updatedProduct.MapToInventory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update inventory {inventoryId}: {message}", inventoryId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting inventory with id: {inventoryId}", id);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _inventoryRepository.DeleteAsync(id, cancellationToken);
            _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Inventory {inventoryId} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting inventory {inventoryId}: {message}", id, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<InventoryResponse> IncreamentAsync(int addedQuantity,long productId,
        CancellationToken cancellationToken = default)
    {   
        _logger.LogInformation("Increasing inventory quantity by {addedQuantity} for product id: {productId}", addedQuantity, productId);
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        var newQuantity = inventory.Quantity + addedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
            var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Inventory quantity increased by {addedQuantity} for product id: {productId} successfully.", addedQuantity, productId);
            return updatedRow.MapToInventory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error increasing inventory quantity by {addedQuantity} for product id: {productId}: {message}", addedQuantity, productId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<InventoryResponse> DecrementAsync(int substractedQuantity, long productId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Decreasing inventory quantity by {substractedQuantity} for product id: {productId}", substractedQuantity, productId);
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if (inventory.Quantity < substractedQuantity) throw new BadRequestException($"Substracted Quantity ({substractedQuantity}) is superior to the actual quantity ({inventory.Quantity})");
        var newQuantity = inventory.Quantity - substractedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
            var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Inventory quantity decreased by {substractedQuantity} for product id: {productId} successfully.", substractedQuantity, productId);
            return updatedRow.MapToInventory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error decreasing inventory quantity by {substractedQuantity} for product id: {productId}: {message}", substractedQuantity, productId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}