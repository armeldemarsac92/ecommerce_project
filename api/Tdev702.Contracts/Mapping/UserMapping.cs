using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class UserMapping
{
    public static UserResponse MapToUserResponse(this UserSQLResponse userSqlResponse)
    {
        return new UserResponse()
        {
            Email = userSqlResponse.Email,
            EmailConfirmed = userSqlResponse.EmailConfirmed,
            LockoutEnabled = userSqlResponse.LockoutEnabled,
            PhoneNumber = userSqlResponse.PhoneNumber,
            Role = userSqlResponse.Role,
            Username = userSqlResponse.Username
        };
    }

    public static List<UserResponse> MapToUsersResponse(this List<UserSQLResponse> userSqlResponse)
    {
        return userSqlResponse.Select(MapToUserResponse).ToList();
    }
}