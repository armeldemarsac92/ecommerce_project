using System.Text.Json.Serialization;

namespace Tdev702.Contracts.API.Request.Payment;

public class CreatePaymentRequest
{
    [JsonPropertyName("payment_method_id")]
    public string? PaymentMethodId { get; init; }

    // public string Currency { get; } = "eur";
    // public bool Confirm { get; } = true;
    // public List<string> PaymentMethodTypes = new List<string> { "card" };

}