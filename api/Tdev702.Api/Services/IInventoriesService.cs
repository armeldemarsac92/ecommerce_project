using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IInventoriesService
{
    public Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);  
    public Task<InventoryResponse> IncreamentAsync(long addedQuantity, long productId, CancellationToken cancellationToken = default);
    public Task<InventoryResponse> SubstractAsync(long substractedQuantity ,long productId, CancellationToken cancellationToken = default);
}

public class InventoriesService : IInventoriesService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;

    public InventoriesService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
    }
    
    public async Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Inventory {id} not found");
        
        return response.MapToInventory();
    }

    public async Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToInventories() :  throw new NotFoundException("Inventories not found");
    }

    public async Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if(response is null) throw new NotFoundException($"The inventory for this product id: {productId} was not found");
        return response.MapToInventory();
    }

    public async Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.CreateAsync(createProductInventoryRequest, cancellationToken);
        return response.MapToInventory();
    }

    public async Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default)
    {
        updateInventoryRequest.Id = id;
        var sqlRequest = updateInventoryRequest.MapToInventoryRequest();
        var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Inventory {id} not found");
        
        var updatedProduct = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        return updatedProduct.MapToInventory();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _inventoryRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<InventoryResponse> IncreamentAsync(long addedQuantity,long productId,
        CancellationToken cancellationToken = default)
    {   
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        var newQuantity = inventory.Quantity + addedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };
        var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
        var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
        return updatedRow.MapToInventory();
    }

    public async Task<InventoryResponse> SubstractAsync(long substractedQuantity, long productId,
        CancellationToken cancellationToken = default)
    {
        var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if (inventory.Quantity < substractedQuantity) throw new BadRequestException($"Substracted Quantity ({substractedQuantity}) is superior to the actual quantity ({inventory.Quantity})");
        var newQuantity = inventory.Quantity - substractedQuantity;
        var sqlRequest = new UpdateInventorySQLRequest()
        {
            Id = inventory.Id,
            Quantity = newQuantity,
        };
        var affectedRows = await _inventoryRepository.UpdateAsync(sqlRequest, cancellationToken);
        if (affectedRows == 0) throw new NotFoundException($"Inventory for the following product: {productId} not found");
        var updatedRow = await _inventoryRepository.GetByIdAsync(inventory.Id, cancellationToken);
        return updatedRow.MapToInventory();
    }
}