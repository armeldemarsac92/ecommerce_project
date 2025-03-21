using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IInventoryRepository
{
    public Task<InventorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<InventorySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

    public Task<InventorySQLResponse?> GetInventoryByProductIdAsync(long productId,
        CancellationToken cancellationToken = default);

    public Task<int> CreateAsync(CreateInventorySQLRequest createInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task<int> UpdateAsync(UpdateInventorySQLRequest updateInventoryRequest,
        CancellationToken cancellationToken = default);

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class InventoryRepository : IInventoryRepository
{
    private readonly IDbContext _dbContext;

    public InventoryRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<InventorySQLResponse>(InventoryQueries.GetInventoryById,
            new { Id = id }, cancellationToken);
    }

    public async Task<List<InventorySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response =
            await _dbContext.QueryAsync<InventorySQLResponse>(InventoryQueries.GetAllInventories, queryOptions, cancellationToken);
        return response.Any() ? response.ToList() : new List<InventorySQLResponse>();
    }

    public async Task<InventorySQLResponse?> GetInventoryByProductIdAsync(long productId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<InventorySQLResponse>(InventoryQueries.GetInventoryByProductId,
            new { ProductId = productId }, cancellationToken);
    }

    public async Task<int> CreateAsync(CreateInventorySQLRequest createInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(InventoryQueries.CreateInventory,
            createInventoryRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateInventorySQLRequest updateInventoryRequest,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(InventoryQueries.UpdateInventory, updateInventoryRequest,
            cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbContext.ExecuteAsync(InventoryQueries.DeleteInventory, new { Id = id }, cancellationToken);
    }
}