using System.Text.Json.Serialization;

namespace Tdev702.Auth.Models;

public class ResendConfirmationRequest
{
    [JsonPropertyName("email" )]
    public required string Email { get; set; }
}