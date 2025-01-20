using System.Text.Json.Serialization;

namespace Tdev702.Contracts.SQL.Request.Shop.Payment;

public class CreatePaymentRequest
{
    [JsonPropertyName("amount")]
    public required long Amount { get; init; }

    [JsonPropertyName("payment_method_id")]
    public string? PaymentMethodId { get; init; }

    // public string Currency { get; } = "eur";
    // public bool Confirm { get; } = true;
    // public List<string> PaymentMethodTypes = new List<string> { "card" };

}