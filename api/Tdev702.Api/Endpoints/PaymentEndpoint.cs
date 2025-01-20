using Microsoft.AspNetCore.Identity;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.Request.Shop.Payment;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class PaymentEndpoint
{
    private const string ContentType = "application/json";

    private static async Task<IResult> CreatePayment(
        HttpContext context,
        IStripePaymentIntentService stripePaymentIntentService,
        CreatePaymentRequest createPaymentRequest,
        UserManager<User> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user == null) throw new BadRequestException("Unknown user");
        if (user.StripeCustomerId == null) throw new BadRequestException("User does not have a Stripe customer ID");
        var request = createPaymentRequest.ToStripePaymentIntentOptions(user.StripeCustomerId);
        var paymentIntent = await stripePaymentIntentService.CreateAsync(request, null, cancellationToken);

        return Results.Ok(paymentIntent);
    }
}