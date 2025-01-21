using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IUserRepository
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
    public Task<List<UserSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<UserSQLResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<UserSQLResponse?> GetUserRole(string userId, CancellationToken cancellationToken = default);
}

public class UserRepository : IUserRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public UserRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryFirstOrDefaultAsync<string>(UserQueries.GetUserById, new { UserId = userId }, cancellationToken);
        return response != null;
    }

    public async Task<List<UserSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<UserSQLResponse>(UserQueries.GetAllUsers, cancellationToken);
        return response.Any() ? response.ToList() : new List<UserSQLResponse>();
    }

    public async Task<UserSQLResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<UserSQLResponse>(UserQueries.GetUserByEmail, new { Email = email }, cancellationToken);
    }

    public async Task<UserSQLResponse?> GetUserRole(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<UserSQLResponse>(UserQueries.GetUserRole, new { Id = userId }, cancellationToken);
    }
}