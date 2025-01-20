using System.Text.Json.Serialization;

namespace Tdev702.Contracts.Auth;

public class Setup2FaRequest
{
    [JsonPropertyName("phone_number" )]
    public string? PhoneNumber { get; set; }  // Only needed for SMS
}