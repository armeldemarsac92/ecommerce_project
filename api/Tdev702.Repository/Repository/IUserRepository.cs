using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IUserRepository
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
}

public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryFirstOrDefaultAsync<string>(UserQueries.GetUserById, new { UserId = userId }, cancellationToken);
        return response != null;
    }
}