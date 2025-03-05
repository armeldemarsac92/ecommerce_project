using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Request;

public class LoginUserRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
    
    [JsonPropertyName("two_factor_code" )]
    public required string TwoFactorCode { get; set; }
}