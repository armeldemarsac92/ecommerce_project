using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth;

public class RegisterUserRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
    
    [JsonPropertyName("first_name" )]
    public required string FirstName { get; set; }
    
    [JsonPropertyName("last_name" )]
    public required string LastName { get; set; }
    
    [JsonPropertyName("password" )]
    public required string Password { get; set; }
}