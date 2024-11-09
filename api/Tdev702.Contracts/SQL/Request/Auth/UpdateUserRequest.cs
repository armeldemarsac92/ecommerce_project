namespace Tdev702.Contracts.SQL.Request.Auth;


public class UpdateUserRequest : CreateUserRequest
{
    public DateTimeOffset? LockoutEnd { get; set; }
}