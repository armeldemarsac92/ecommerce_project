namespace Tdev702.Repository.SQL;

public class UserQueries
{
    public static string GetUserById = @"
    SELECT ""UserName""
    FROM tdev700.auth.""AspNetUsers""
    WHERE ""Id"" = @UserId;";
}











































