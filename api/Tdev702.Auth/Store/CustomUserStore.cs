using Microsoft.AspNetCore.Identity;
using Tdev702.Contracts.API.Auth;
using Tdev702.Repository.Repository;

namespace Tdev702.Auth.Store;

public class CustomUserStore : IUserStore<ApplicationUser>,
                             IUserPasswordStore<ApplicationUser>,
                             IUserEmailStore<ApplicationUser>,
                             IUserSecurityStampStore<ApplicationUser>,
                             IUserLockoutStore<ApplicationUser>,
                             IUserLoginStore<ApplicationUser>
{
    private readonly IIdentityRepository<ApplicationUser> _authRepository;

    public CustomUserStore(IIdentityRepository<ApplicationUser> authRepository)
    {
        _authRepository = authRepository;
    }

    #region IUserStore Implementation
    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.CreateAsync(user, cancellationToken);

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.DeleteAsync(user, cancellationToken);

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        => await _authRepository.FindByIdAsync(userId, cancellationToken);

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        => await _authRepository.FindByNameAsync(normalizedUserName, cancellationToken);

    public async Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetNormalizedUserNameAsync(user, cancellationToken);

    public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetUserIdAsync(user, cancellationToken);

    public async Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetUserNameAsync(user, cancellationToken);

    public async Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        => await _authRepository.SetNormalizedUserNameAsync(user, normalizedName, cancellationToken);

    public async Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        => await _authRepository.SetUserNameAsync(user, userName, cancellationToken);

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.UpdateAsync(user, cancellationToken);
    #endregion

    #region IUserPasswordStore Implementation
    public async Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetPasswordHashAsync(user, cancellationToken);

    public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.HasPasswordAsync(user, cancellationToken);

    public async Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        => await _authRepository.SetPasswordHashAsync(user, passwordHash, cancellationToken);
    #endregion

    #region IUserEmailStore Implementation
    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        => await _authRepository.FindByEmailAsync(normalizedEmail, cancellationToken);

    public async Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetEmailAsync(user, cancellationToken);

    public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetEmailConfirmedAsync(user, cancellationToken);

    public async Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetNormalizedEmailAsync(user, cancellationToken);

    public async Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        => await _authRepository.SetEmailAsync(user, email, cancellationToken);

    public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        => await _authRepository.SetEmailConfirmedAsync(user, confirmed, cancellationToken);

    public async Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        => await _authRepository.SetNormalizedEmailAsync(user, normalizedEmail, cancellationToken);
    #endregion

    #region IUserSecurityStampStore Implementation
    public async Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetSecurityStampAsync(user, cancellationToken);

    public async Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        => await _authRepository.SetSecurityStampAsync(user, stamp, cancellationToken);
    #endregion

    #region IUserLockoutStore Implementation
    public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetAccessFailedCountAsync(user, cancellationToken);

    public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetLockoutEnabledAsync(user, cancellationToken);

    public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.GetLockoutEndDateAsync(user, cancellationToken);

    public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.IncrementAccessFailedCountAsync(user, cancellationToken);

    public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        => await _authRepository.ResetAccessFailedCountAsync(user, cancellationToken);

    public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        => await _authRepository.SetLockoutEnabledAsync(user, enabled, cancellationToken);

    public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        => await _authRepository.SetLockoutEndDateAsync(user, lockoutEnd, cancellationToken);
    #endregion
    
    #region IUserLoginStore Implementation
    public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        await _authRepository.AddUserLoginAsync(user.Id, login, cancellationToken);
    }

    public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        await _authRepository.RemoveUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await _authRepository.GetUserLoginsAsync(user.Id, cancellationToken);
    }

    public async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return await _authRepository.FindByLoginAsync(loginProvider, providerKey, cancellationToken);
    }
    #endregion

    #region IDisposable Implementation
    public void Dispose()
    {
        // Nothing to dispose
        GC.SuppressFinalize(this);
    }
    #endregion
}