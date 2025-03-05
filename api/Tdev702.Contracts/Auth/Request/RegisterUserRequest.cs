using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Request;

public class RegisterUserRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
}