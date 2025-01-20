using Microsoft.AspNetCore.Identity;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Auth.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Payment;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints.Shop;

public static class PaymentEndpoint
{
    private const string ContentType = "application/json";
    private const string Tags = "Payments";

    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ShopRoutes.Payments.Create, CreatePayment)
            .WithTags(Tags)
            .WithDescription("Create a new payment")
            .Produces<PaymentIntent>(200)
            .Produces(400);

        return app;
    }

    private static async Task<IResult> CreatePayment(
        HttpContext context,
        IStripePaymentIntentService stripePaymentIntentService,
        CreatePaymentRequest createPaymentRequest,
        UserManager<User> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user == null) throw new BadRequestException("Unknown user");
        var request = createPaymentRequest.ToStripePaymentIntentOptions(user.StripeCustomerId);
        var paymentIntent = await stripePaymentIntentService.CreateAsync(request, null, cancellationToken);

        return Results.Ok(paymentIntent);
    }
}