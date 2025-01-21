using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IUserRepository
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
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
}