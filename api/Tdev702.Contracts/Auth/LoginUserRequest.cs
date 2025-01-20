using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth;

public class LoginUserRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
    
    [JsonPropertyName("password" )]
    public required string Password { get; set; }
    
    [JsonPropertyName("two_factor_code" )]
    public string? TwoFactorCode { get; set; }
}