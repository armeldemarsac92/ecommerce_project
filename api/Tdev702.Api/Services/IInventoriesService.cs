using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Request.Shop.Inventory;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IInventoriesService
{
    public Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);

    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateProductInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    public Task<InventoryResponse> IncreamentAsync(long addedQuantity, long productId,
        CancellationToken cancellationToken = default);

    public Task<InventoryResponse> SubstractAsync(long substractedQuantity, long productId,
        CancellationToken cancellationToken = default);
}

public class InventoriesService : IInventoriesService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger<InventoriesService> _logger;

    public InventoriesService(IInventoryRepository inventoryRepository, ILogger<InventoriesService> logger)
    {
        _inventoryRepository = inventoryRepository;
        _logger = logger;
    }

    public async Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to fetch Inventory: {id}", id);
            var response = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
            if (response is null) throw new NotFoundException($"Inventory {id} not found");
            _logger.LogInformation("Successfully fetched Inventory: {id}", id);
            return response.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching Inventory by id: {Message}", e.Message);
            throw;
        }
    }

    public async Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to fetch all Inventories");
            var response = await _inventoryRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched all Inventories");
            return response.Any() ? response.MapToInventories() : throw new NotFoundException("Inventories not found");
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching all Inventories: {Message}", e.Message);
            throw;
        }
    }

    public async Task<InventoryResponse> GetByProductIdAsync(long productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to fetch Inventory with product id: {productId}", productId);
            var response = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
            if (response is null)
                throw new NotFoundException($"The inventory for this product id: {productId} was not found");
            _logger.LogInformation("Successfully fetched Inventory with product id: {productId}", productId);
            return response.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching Inventory by product id: {ProductId}: {Message}", productId, e.Message);
            throw;
        }
    }

    public async Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to create Inventory for Product: {ProductId}",
                createProductInventoryRequest.ProductId);
            var response = await _inventoryRepository.CreateAsync(createProductInventoryRequest, cancellationToken);
            _logger.LogInformation("Successfully created Inventory for Product: {ProductId}",
                createProductInventoryRequest.ProductId);
            return response.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating Inventory for Product id: {ProductId}: {Message}",
                createProductInventoryRequest.ProductId, e.Message);
            throw;
        }
    }

    public async Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            updateInventoryRequest.Id = id;
            _logger.LogInformation("Trying to update Inventory: {Id}", updateInventoryRequest.Id);
            var sqlRequest = updateInventoryRequest.MapToInventoryRequest();
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);

            if (affectedRows == 0) throw new NotFoundException($"Inventory {id} not found");

            var updatedProduct = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
            _logger.LogInformation("Successfully update Inventory: {ProductId}", updateInventoryRequest.Id);
            return updatedProduct.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating Inventory: {Message}", e.Message);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to delete Inventory: {Id}", id);
            await _inventoryRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted Inventory: {Id}", id);
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting Inventory: {Message}", e.Message);
            throw;
        }
    }

    public async Task<InventoryResponse> IncreamentAsync(long addedQuantity, long productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
            var newQuantity = inventory.Quantity + addedQuantity;
            _logger.LogInformation("Trying to add {Quantity} to Inventory for the following product: {ProductId}", newQuantity, productId);
            var sqlRequest = new UpdateInventorySQLRequest()
            {
                Id = inventory.Id,
                Quantity = newQuantity,
            };
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0)
                throw new NotFoundException($"Inventory for the following product: {productId} not found");
            _logger.LogInformation("Successfully updated Inventory for the following product: {ProductId}", productId);
            var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
            return updatedRow.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating Inventory: {Message}", e.Message);
            throw;
        }
    }

    public async Task<InventoryResponse> SubstractAsync(long substractedQuantity, long productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to substract {Quantity} to Inventory for the following product: {ProductId}",
                substractedQuantity, productId);
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
            if (inventory.Quantity < substractedQuantity)
                throw new BadRequestException(
                    $"Substracted Quantity ({substractedQuantity}) is superior to the actual quantity ({inventory.Quantity})");
            var newQuantity = inventory.Quantity - substractedQuantity;
            var sqlRequest = new UpdateInventorySQLRequest()
            {
                Id = inventory.Id,
                Quantity = newQuantity,
            };
            var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0)
                throw new NotFoundException($"Inventory for the following product: {productId} not found");
            var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
            _logger.LogInformation("Successfully updated Inventory for the following product: {ProductId}", productId);
            return updatedRow.MapToInventory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating Inventory: {Message}", e.Message);
            throw;
        }
    }
}