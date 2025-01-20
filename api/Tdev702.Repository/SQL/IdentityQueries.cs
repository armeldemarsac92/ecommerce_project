namespace Tdev702.Repository.SQL;

public static class IdentityQueries
{
    #region User Store Queries
    public const string CreateUser = @"
    INSERT INTO auth.users (
        Id, 
        UserName, 
        NormalizedUserName, 
        Email, 
        NormalizedEmail, 
        EmailConfirmed,
        PasswordHash, 
        SecurityStamp,
        ConcurrencyStamp, 
        PhoneNumber,
        PhoneNumberConfirmed,
        TwoFactorEnabled, 
        LockoutEnd, 
        LockoutEnabled, 
        AccessFailedCount
    )
    VALUES (
        @Id, 
        @UserName, 
        @NormalizedUserName, 
        @Email, 
        @NormalizedEmail, 
        @EmailConfirmed,
        @PasswordHash, 
        @SecurityStamp,
        @ConcurrencyStamp, 
        @PhoneNumber,
        @PhoneNumberConfirmed,
        @TwoFactorEnabled, 
        @LockoutEnd, 
        @LockoutEnabled, 
        @AccessFailedCount
    )";

    public const string UpdateUser = @"
    UPDATE auth.users 
    SET UserName = @UserName,
        NormalizedUserName = @NormalizedUserName,
        Email = @Email,
        NormalizedEmail = @NormalizedEmail,
        EmailConfirmed = @EmailConfirmed,
        PasswordHash = @PasswordHash,
        SecurityStamp = @SecurityStamp,
        ConcurrencyStamp = @ConcurrencyStamp,
        PhoneNumber = @PhoneNumber,
        PhoneNumberConfirmed = @PhoneNumberConfirmed,
        TwoFactorEnabled = @TwoFactorEnabled,
        LockoutEnd = @LockoutEnd,
        LockoutEnabled = @LockoutEnabled,
        AccessFailedCount = @AccessFailedCount
    WHERE Id = @Id";

    public const string DeleteUser = "DELETE FROM auth.users WHERE Id = @Id";

    public const string FindUserById = "SELECT * FROM auth.users WHERE Id = @Id";
    
    public const string FindUserByName = "SELECT * FROM auth.users WHERE NormalizedUserName = @NormalizedUserName";
    
    public const string FindUserByEmail = "SELECT * FROM auth.users WHERE NormalizedEmail = @NormalizedEmail";
    #endregion

    #region User Email Store Queries
    public const string GetEmail = "SELECT Email FROM auth.users WHERE Id = @Id";
    
    public const string GetEmailConfirmed = "SELECT EmailConfirmed FROM auth.users WHERE Id = @Id";
    
    public const string GetNormalizedEmail = "SELECT NormalizedEmail FROM auth.users WHERE Id = @Id";
    
    public const string SetEmail = "UPDATE auth.users SET Email = @Email WHERE Id = @Id";
    
    public const string SetEmailConfirmed = "UPDATE auth.users SET EmailConfirmed = @EmailConfirmed WHERE Id = @Id";
    
    public const string SetNormalizedEmail = "UPDATE auth.users SET NormalizedEmail = @NormalizedEmail WHERE Id = @Id";
    #endregion

    #region User Password Store Queries
    public const string GetPasswordHash = "SELECT PasswordHash FROM auth.users WHERE Id = @Id";
    
    public const string SetPasswordHash = "UPDATE auth.users SET PasswordHash = @PasswordHash WHERE Id = @Id";
    
    public const string HasPassword = "SELECT CASE WHEN PasswordHash IS NULL THEN 0 ELSE 1 END FROM auth.users WHERE Id = @Id";
    #endregion

    #region User Security Stamp Queries
    public const string GetSecurityStamp = "SELECT SecurityStamp FROM auth.users WHERE Id = @Id";
    
    public const string SetSecurityStamp = "UPDATE auth.users SET SecurityStamp = @SecurityStamp WHERE Id = @Id";
    #endregion

    #region User Lockout Queries
    public const string GetAccessFailedCount = "SELECT AccessFailedCount FROM auth.users WHERE Id = @Id";
    
    public const string GetLockoutEnabled = "SELECT LockoutEnabled FROM auth.users WHERE Id = @Id";
    
    public const string GetLockoutEndDate = "SELECT LockoutEnd FROM auth.users WHERE Id = @Id";
    
    public const string IncrementAccessFailedCount = @"
        UPDATE auth.users 
        SET AccessFailedCount = AccessFailedCount + 1 
        WHERE Id = @Id;
        SELECT AccessFailedCount FROM auth.users WHERE Id = @Id;";
    
    public const string ResetAccessFailedCount = "UPDATE auth.users SET AccessFailedCount = 0 WHERE Id = @Id";
    
    public const string SetLockoutEnabled = "UPDATE auth.users SET LockoutEnabled = @LockoutEnabled WHERE Id = @Id";
    
    public const string SetLockoutEndDate = "UPDATE auth.users SET LockoutEnd = @LockoutEnd WHERE Id = @Id";
    #endregion

    #region User Name Store Queries
    public const string GetUserName = "SELECT UserName FROM auth.users WHERE Id = @Id";
    
    public const string SetUserName = "UPDATE auth.users SET UserName = @UserName WHERE Id = @Id";
    
    public const string GetNormalizedUserName = "SELECT NormalizedUserName FROM auth.users WHERE Id = @Id";
    
    public const string SetNormalizedUserName = "UPDATE auth.users SET NormalizedUserName = @NormalizedUserName WHERE Id = @Id";
    
    public const string GetUserId = "SELECT Id FROM auth.users WHERE Id = @Id";
    #endregion
    
    #region External Login Queries
    public const string AddUserLogin = @"
        INSERT INTO auth.user_logins (LoginProvider, ProviderKey, ProviderDisplayName, UserId)
        VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId)";

    public const string RemoveUserLogin = @"
        DELETE FROM auth.user_logins 
        WHERE UserId = @UserId 
        AND LoginProvider = @LoginProvider 
        AND ProviderKey = @ProviderKey";

    public const string GetUserLogins = @"
        SELECT LoginProvider, ProviderKey, ProviderDisplayName
        FROM auth.user_logins
        WHERE UserId = @UserId";

    public const string FindByLogin = @"
        SELECT u.*
        FROM auth.users u
        INNER JOIN auth.user_logins ul ON u.Id = ul.UserId
        WHERE ul.LoginProvider = @LoginProvider 
        AND ul.ProviderKey = @ProviderKey";
    #endregion
}