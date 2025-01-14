using System.Text.Json.Serialization;

namespace Tdev702.Auth.Models;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}