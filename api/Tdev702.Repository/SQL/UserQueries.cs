namespace Tdev702.Repository.SQL;

public class UserQueries
{
    public static string GetUserById = @"
    SELECT ""UserName""
    FROM tdev700.auth.""AspNetUsers""
    WHERE ""Id"" = @UserId;";
    
    public static string GetUserByEmail = @"
    SELECT ""Email""
    From tdev700.auth.""AspNetUsers""
    WHERE ""Email"" = @Email;";

    public static string GetAllUsers = @"
    SELECT ""UserName""
    FROM tdev700.auth.""AspNetUsers""
    ORDER BY ""Id"" = @UserId
    LIMIT 100 OFFSET (@page_number - 1) * 100;";

    public static string GetUserRole = @"
    SELECT ""RoleId""
    FROM tdev700.auth.""AspNetUserRoles""
    WHERE ""UserId"" = @UserId;";
}











































