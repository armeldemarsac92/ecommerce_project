using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IUserService
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
    public Task<UserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
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
        var role = await _userRepository.GetUserRole(userId, cancellationToken);
        return await _userRepository.UserExistsAsync(userId, cancellationToken);
    }
    public async Task<UserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var response = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return response.MapToUserResponse();
    }
    public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _userRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToUsersResponse() : throw new NotFoundException("Users not found");
    }
}