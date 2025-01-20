using Microsoft.AspNetCore.Identity;
using Tdev702.Contracts.Auth;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IIdentityRepository<TUser> where TUser : class
{
    // User Store
    Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default);
    Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default);
    Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default);
    Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);
    Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);
    
    // User Email Store
    Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken = default);
    Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default);
    Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default);
    Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken = default);
    Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default);
    Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken = default);
    
    // User Password Store
    Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default);
    Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default);
    Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken = default);
    
    // User Security Stamp Store
    Task<string?> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default);
    Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default);
    
    // User Lockout Store
    Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default);
    Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken = default);
    Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken = default);
    Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default);
    Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default);
    Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default);
    Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default);
    
    // User Name Store
    Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default);
    Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default);
    Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default);
    Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken = default);
    Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken = default);
    
    // External Login Store
    Task AddUserLoginAsync(string userId, UserLoginInfo login, CancellationToken cancellationToken = default);
    Task RemoveUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken);
    Task<IList<UserLoginInfo>> GetUserLoginsAsync(string userId, CancellationToken cancellationToken);
    Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
}

public class IdentityRepository<TUser> : IIdentityRepository<TUser> where TUser : ApplicationUser
{
    private readonly IDBSQLCommand _dbCommand;

    public IdentityRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    #region User Store Implementation
    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbCommand.ExecuteAsync(IdentityQueries.CreateUser, user, cancellationToken);
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            var affected = await _dbCommand.ExecuteAsync(IdentityQueries.UpdateUser, user, cancellationToken);
            return affected > 0 
                ? IdentityResult.Success 
                : IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            var affected = await _dbCommand.ExecuteAsync(IdentityQueries.DeleteUser, new { user.Id }, cancellationToken);
            return affected > 0 
                ? IdentityResult.Success 
                : IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TUser>(
            IdentityQueries.FindUserById, 
            new { Id = userId }, 
            cancellationToken);
    }

    public async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TUser>(
            IdentityQueries.FindUserByName, 
            new { NormalizedUserName = normalizedUserName }, 
            cancellationToken);
    }

    public async Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<TUser>(
            IdentityQueries.FindUserByEmail, 
            new { NormalizedEmail = normalizedEmail }, 
            cancellationToken);
    }
    #endregion

    #region Email Store Implementation
    public async Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetEmail, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<bool>(
            IdentityQueries.GetEmailConfirmed, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetNormalizedEmail, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetEmail, 
            new { user.Id, Email = email }, 
            cancellationToken);
    }

    public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetEmailConfirmed, 
            new { user.Id, EmailConfirmed = confirmed }, 
            cancellationToken);
    }

    public async Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetNormalizedEmail, 
            new { user.Id, NormalizedEmail = normalizedEmail }, 
            cancellationToken);
    }
    #endregion

    #region Password Store Implementation
    public async Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetPasswordHash, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<bool>(
            IdentityQueries.HasPassword, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetPasswordHash, 
            new { user.Id, PasswordHash = passwordHash }, 
            cancellationToken);
    }
    #endregion

    #region Security Stamp Store Implementation
    public async Task<string?> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetSecurityStamp, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetSecurityStamp, 
            new { user.Id, SecurityStamp = stamp }, 
            cancellationToken);
    }
    #endregion

    #region Lockout Store Implementation
    public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<int>(
            IdentityQueries.GetAccessFailedCount, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<bool>(
            IdentityQueries.GetLockoutEnabled, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<DateTimeOffset?>(
            IdentityQueries.GetLockoutEndDate, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<int>(
            IdentityQueries.IncrementAccessFailedCount, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.ResetAccessFailedCount, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetLockoutEnabled, 
            new { user.Id, LockoutEnabled = enabled }, 
            cancellationToken);
    }

    public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetLockoutEndDate, 
            new { user.Id, LockoutEnd = lockoutEnd }, 
            cancellationToken);
    }
    #endregion

    #region User Name Store Implementation
    public async Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetUserName, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetNormalizedUserName, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<string>(
            IdentityQueries.GetUserId, 
            new { user.Id }, 
            cancellationToken);
    }

    public async Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetUserName, 
            new { user.Id, UserName = userName }, 
            cancellationToken);
    }

    public async Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.SetNormalizedUserName, 
            new { user.Id, NormalizedUserName = normalizedName }, 
            cancellationToken);
    }
    #endregion
    
    public async Task AddUserLoginAsync(string userId, UserLoginInfo login, CancellationToken cancellationToken)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.AddUserLogin,
            new
            {
                UserId = userId,
                login.LoginProvider,
                login.ProviderKey,
                login.ProviderDisplayName
            },
            cancellationToken);
    }

    public async Task RemoveUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        await _dbCommand.ExecuteAsync(
            IdentityQueries.RemoveUserLogin,
            new { UserId = userId, LoginProvider = loginProvider, ProviderKey = providerKey },
            cancellationToken);
    }

    public async Task<IList<UserLoginInfo>> GetUserLoginsAsync(string userId, CancellationToken cancellationToken)
    {
        var logins = await _dbCommand.QueryAsync<UserLoginInfoData>(
            IdentityQueries.GetUserLogins,
            new { UserId = userId },
            cancellationToken);

        return logins.Select(l => new UserLoginInfo(
            l.LoginProvider,
            l.ProviderKey,
            l.ProviderDisplayName)).ToList();
    }

    public async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<ApplicationUser>(
            IdentityQueries.FindByLogin,
            new { LoginProvider = loginProvider, ProviderKey = providerKey },
            cancellationToken);
    }
    
    private class UserLoginInfoData
    {
        public string LoginProvider { get; set; } = default!;
        public string ProviderKey { get; set; } = default!;
        public string? ProviderDisplayName { get; set; }
    }
}

