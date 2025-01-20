using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Request;

public class RefreshTokenRequest
{
    [JsonPropertyName("refresh_token" )]
    public required string RefreshToken { get; set; }
}