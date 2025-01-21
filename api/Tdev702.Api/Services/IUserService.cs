using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IUserService
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.UserExistsAsync(userId, cancellationToken);
    }
}