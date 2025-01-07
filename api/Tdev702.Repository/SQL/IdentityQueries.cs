namespace Tdev702.Repository.SQL;

public static class IdentityQueries
{
    #region User Store Queries
    public const string CreateUser = @"
    INSERT INTO Users (
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
    UPDATE Users 
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

    public const string DeleteUser = "DELETE FROM Users WHERE Id = @Id";

    public const string FindUserById = "SELECT * FROM Users WHERE Id = @Id";
    
    public const string FindUserByName = "SELECT * FROM Users WHERE NormalizedUserName = @NormalizedUserName";
    
    public const string FindUserByEmail = "SELECT * FROM Users WHERE NormalizedEmail = @NormalizedEmail";
    #endregion

    #region User Email Store Queries
    public const string GetEmail = "SELECT Email FROM Users WHERE Id = @Id";
    
    public const string GetEmailConfirmed = "SELECT EmailConfirmed FROM Users WHERE Id = @Id";
    
    public const string GetNormalizedEmail = "SELECT NormalizedEmail FROM Users WHERE Id = @Id";
    
    public const string SetEmail = "UPDATE Users SET Email = @Email WHERE Id = @Id";
    
    public const string SetEmailConfirmed = "UPDATE Users SET EmailConfirmed = @EmailConfirmed WHERE Id = @Id";
    
    public const string SetNormalizedEmail = "UPDATE Users SET NormalizedEmail = @NormalizedEmail WHERE Id = @Id";
    #endregion

    #region User Password Store Queries
    public const string GetPasswordHash = "SELECT PasswordHash FROM Users WHERE Id = @Id";
    
    public const string SetPasswordHash = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id";
    
    public const string HasPassword = "SELECT CASE WHEN PasswordHash IS NULL THEN 0 ELSE 1 END FROM Users WHERE Id = @Id";
    #endregion

    #region User Security Stamp Queries
    public const string GetSecurityStamp = "SELECT SecurityStamp FROM Users WHERE Id = @Id";
    
    public const string SetSecurityStamp = "UPDATE Users SET SecurityStamp = @SecurityStamp WHERE Id = @Id";
    #endregion

    #region User Lockout Queries
    public const string GetAccessFailedCount = "SELECT AccessFailedCount FROM Users WHERE Id = @Id";
    
    public const string GetLockoutEnabled = "SELECT LockoutEnabled FROM Users WHERE Id = @Id";
    
    public const string GetLockoutEndDate = "SELECT LockoutEnd FROM Users WHERE Id = @Id";
    
    public const string IncrementAccessFailedCount = @"
        UPDATE Users 
        SET AccessFailedCount = AccessFailedCount + 1 
        WHERE Id = @Id;
        SELECT AccessFailedCount FROM Users WHERE Id = @Id;";
    
    public const string ResetAccessFailedCount = "UPDATE Users SET AccessFailedCount = 0 WHERE Id = @Id";
    
    public const string SetLockoutEnabled = "UPDATE Users SET LockoutEnabled = @LockoutEnabled WHERE Id = @Id";
    
    public const string SetLockoutEndDate = "UPDATE Users SET LockoutEnd = @LockoutEnd WHERE Id = @Id";
    #endregion

    #region User Name Store Queries
    public const string GetUserName = "SELECT UserName FROM Users WHERE Id = @Id";
    
    public const string SetUserName = "UPDATE Users SET UserName = @UserName WHERE Id = @Id";
    
    public const string GetNormalizedUserName = "SELECT NormalizedUserName FROM Users WHERE Id = @Id";
    
    public const string SetNormalizedUserName = "UPDATE Users SET NormalizedUserName = @NormalizedUserName WHERE Id = @Id";
    
    public const string GetUserId = "SELECT Id FROM Users WHERE Id = @Id";
    #endregion
    
    #region External Login Queries
    public const string AddUserLogin = @"
        INSERT INTO UserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId)
        VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId)";

    public const string RemoveUserLogin = @"
        DELETE FROM UserLogins 
        WHERE UserId = @UserId 
        AND LoginProvider = @LoginProvider 
        AND ProviderKey = @ProviderKey";

    public const string GetUserLogins = @"
        SELECT LoginProvider, ProviderKey, ProviderDisplayName
        FROM UserLogins
        WHERE UserId = @UserId";

    public const string FindByLogin = @"
        SELECT u.*
        FROM Users u
        INNER JOIN UserLogins ul ON u.Id = ul.UserId
        WHERE ul.LoginProvider = @LoginProvider 
        AND ul.ProviderKey = @ProviderKey";
    #endregion
}