using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Inventory;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IInventoriesService
{
    public Task<Inventory> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Inventory> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task<Inventory> UpdateAsync(long id, UpdateInventoryRequest updateProductInventoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);  
}

public class InventoriesService : IInventoriesService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoriesService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<Inventory> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Inventory {id} not found");
        
        return response.MapToProductInventory();
    }

    public async Task<List<Inventory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _inventoryRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToProductInventories() :  throw new NotFoundException("Inventories not found");
    }

    public async Task<Inventory> CreateAsync(CreateInventoryRequest createProductInventoryRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Inventory> UpdateAsync(long id, UpdateInventoryRequest updateProductInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}