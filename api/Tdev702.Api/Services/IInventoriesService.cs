using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Inventory;
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
}

public class InventoriesService : IInventoriesService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoriesService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<InventoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Inventory {id} not found");
        
        return response.MapToProductInventory();
    }

    public async Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProductInventories() :  throw new NotFoundException("Inventories not found");
    }

    public async Task<InventoryResponse> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetInventoryByProductIdAsync(productId, cancellationToken);
        if(response is null) throw new NotFoundException($"The inventory for this product id: {productId} was not found");
        return response.MapToProductInventory();
    }

    public async Task<InventoryResponse> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.CreateAsync(createProductInventoryRequest, cancellationToken);
        return response.MapToProductInventory();
    }

    public async Task<InventoryResponse> UpdateAsync(long id, UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default)
    {
        updateInventoryRequest.Id = id;
        var affectedRows = await _inventoryRepository.UpdateAsync(updateInventoryRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Inventory {id} not found");
        
        var updatedProduct = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        return updatedProduct.MapToProductInventory();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _inventoryRepository.DeleteAsync(id, cancellationToken);
    }
}