using Microsoft.AspNetCore.Identity;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Utils;
using Tdev702.Contracts.API.Request.Payment;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class PaymentEndpoint
{
    private const string ContentType = "application/json";
    private const string Tags = "Payments";

    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ShopRoutes.Payments.Create, CreatePayment)
            .WithTags(Tags)
            .WithDescription("Create a new payment")
            .RequireAuthorization("Authenticated")
            .Accepts<CreatePaymentRequest>(ContentType)
            .Produces<PaymentIntent>(200)
            .Produces(400);
        
        return app;
    }

    private static async Task<IResult> CreatePayment(
        HttpContext context,
        IStripePaymentIntentService stripePaymentIntentService,
        CreatePaymentRequest createPaymentRequest,
        CancellationToken cancellationToken)
    {
        var userId = context.GetUserStripeIdFromClaims();
        var request = createPaymentRequest.ToStripePaymentIntentOptions(userId);
        var paymentIntent = await stripePaymentIntentService.CreateAsync(request, null, cancellationToken);

        return Results.Ok(paymentIntent);
    }
}