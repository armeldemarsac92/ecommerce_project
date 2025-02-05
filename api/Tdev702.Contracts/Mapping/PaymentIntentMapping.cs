using Stripe;
using Tdev702.Contracts.API.Request.Payment;

namespace Tdev702.Contracts.Mapping;

public static class PaymentIntentMapping
{
    public static PaymentIntentCreateOptions ToStripePaymentIntentOptions(this CreatePaymentRequest request, string customerId, long amount)
    {
        return new PaymentIntentCreateOptions
        {
            Amount = amount*100,
            Currency = "eur",
            PaymentMethod = request.PaymentMethodId,
            Customer = customerId,
            Confirm = request.PaymentMethodId != null,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            
        };
    }
}