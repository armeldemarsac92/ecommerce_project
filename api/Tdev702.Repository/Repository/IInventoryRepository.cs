using Tdev702.Contracts.SQL.Request.Shop.Inventory;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IInventoryRepository
{
    public Task<InventoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<InventoryResponse> GetInventoryByProductIdAsync(long productId,
        CancellationToken cancellationToken = default);

    public Task<InventoryResponse> CreateAsync(CreateInventoryRequest createInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task<int> UpdateAsync(UpdateInventoryRequest updateInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class InventoryRepository : IInventoryRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public InventoryRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<InventoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<InventoryResponse>(InventoryQuerries.GetInventoryById,
            new { Id = id }, cancellationToken);
    }

    public async Task<List<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response =
            await _dbCommand.QueryAsync<InventoryResponse>(InventoryQuerries.GetAllInventory, cancellationToken);
        return response.Any() ? response.ToList() : new List<InventoryResponse>();
    }

    public async Task<InventoryResponse> GetInventoryByProductIdAsync(long productId,
        CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<InventoryResponse>(InventoryQuerries.GetInventoryByProductId,
            new { ProductId = productId }, cancellationToken);
    }

    public async Task<InventoryResponse> CreateAsync(CreateInventoryRequest createInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<InventoryResponse>(InventoryQuerries.CreateInventory,
            createInventoryRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateInventoryRequest updateInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _dbCommand.ExecuteAsync(InventoryQuerries.UpdateInventory, updateInventoryRequest,
            cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(InventoryQuerries.DeleteInventory, new { Id = id }, cancellationToken);
    }
}