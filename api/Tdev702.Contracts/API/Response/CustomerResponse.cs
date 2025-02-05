using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Response;

public class CustomerResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("user_name")]
    public string Username { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("email_confirmed")]
    public bool EmailConfirmed { get; set; }
    
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
    
    [JsonPropertyName("stripe_id")]
    public string StripeId { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("locakout_enabled")]
    public string LockoutEnabled { get; set; }
}