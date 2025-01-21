using Stripe;
using Tdev702.Contracts.API.Request.Payment;

namespace Tdev702.Contracts.Mapping;

public static class PaymentIntentMapping
{
    public static PaymentIntentCreateOptions ToStripePaymentIntentOptions(this CreatePaymentRequest request, string customerId)
    {
        return new PaymentIntentCreateOptions
        {
            Amount = request.Amount,
            Currency = "eur",
            PaymentMethod = request.PaymentMethodId,
            Customer = customerId,
            Confirm = request.PaymentMethodId != null,
            PaymentMethodTypes = new List<string> { "card" },
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };
    }
}