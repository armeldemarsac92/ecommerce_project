using Tdev702.Contracts.SQL.Request.Shop.Inventory;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IInventoryRepository
{
    public Task<InventoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createInventoryRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class InventoryRepository : IInventoryRepository
{
    private readonly IDBSQLCommand _dbCommand;
    
    public InventoryRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }
    
    public Task<InventoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createInventoryRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(UpdateInventoryRequest updateInventoryRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}