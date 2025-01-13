using System.Text.Json.Serialization;

namespace Tdev702.Auth.Models;

public class Setup2FaRequest
{
    [JsonPropertyName("phone_number" )]
    public string? PhoneNumber { get; set; }  // Only needed for SMS
}