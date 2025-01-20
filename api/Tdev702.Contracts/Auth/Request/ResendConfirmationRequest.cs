using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth.Request;

public class ResendConfirmationRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
}