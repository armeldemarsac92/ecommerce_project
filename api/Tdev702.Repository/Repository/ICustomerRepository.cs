using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ICustomerRepository
{
    public Task<bool> CustomerExistsAsync(string userId, CancellationToken cancellationToken = default);
    public Task<CustomerSQLResponse?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
    public Task<List<CustomerSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<long> GetCustomersCountAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbContext _dbContext;

    public CustomerRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CustomerExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryFirstOrDefaultAsync<string>(CustomerQueries.GetCustomerById, new { UserId = userId }, cancellationToken);
        return response != null;
    }

    public async Task<CustomerSQLResponse?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<CustomerSQLResponse>(CustomerQueries.GetCustomerById, new { UserId = userId }, cancellationToken);
    }

    public async Task<List<CustomerSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<CustomerSQLResponse>(CustomerQueries.GetAllCustomers, queryOptions, cancellationToken);
        return response.Any()? response.ToList() : new List<CustomerSQLResponse>();
    }

    public async Task<long> GetCustomersCountAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<long>(CustomerQueries.GetCustomersCountByDateRange,
            new { StartDate = startDate, EndDate = endDate }, cancellationToken);
    }
}