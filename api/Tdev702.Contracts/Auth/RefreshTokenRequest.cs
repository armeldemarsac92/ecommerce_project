using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Tdev702.Auth.Models;

public class RefreshTokenRequest
{
    [JsonPropertyName("refresh_token" )]
    public required string RefreshToken { get; set; }
}