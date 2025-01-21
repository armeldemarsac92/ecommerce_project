namespace Tdev702.Contracts.API.Response;

public class UserResponse
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required bool EmailConfirmed { get; init; }
    public required bool LockoutEnabled { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Role { get; init; }
}