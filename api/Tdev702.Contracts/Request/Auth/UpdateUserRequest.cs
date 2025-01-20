namespace Tdev702.Contracts.Request.Auth;


public class UpdateUserRequest : CreateUserRequest
{
    public DateTimeOffset? LockoutEnd { get; set; }
}