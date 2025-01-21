using Stripe;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Contracts.Mapping;

public static class PaymentMethodMapping
{
    public static PaymentMethodResponse MapToPaymentMethod(this PaymentMethod paymentMethod)
    {
        return new PaymentMethodResponse
        {
            Id = paymentMethod.Id,
            Last4 = paymentMethod.Card.Last4,
            Brand = paymentMethod.Card.Brand,
            ExpiryMonth = paymentMethod.Card.ExpMonth,
            ExpiryYear = paymentMethod.Card.ExpYear
        };
    }
    
    public static List<PaymentMethodResponse> MapToPaymentMethods(this IEnumerable<PaymentMethod> paymentMethods)
    {
        return paymentMethods.Select(MapToPaymentMethod).ToList();
    }
}