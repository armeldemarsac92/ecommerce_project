using Microsoft.AspNetCore.Identity;
using Stripe;
using Stripe.Checkout;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Api.Utils;
using Tdev702.Contracts.API.Request.Payment;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class PaymentEndpoint
{
    private const string Tags = "Payments";

    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        // app.MapPost(ShopRoutes.Orders.CreatePayment, CreatePaymentIntent)
        //     .WithTags(Tags)
        //     .WithDescription("Create a new payment")
        //     .RequireAuthorization("Authenticated")
        //     .Accepts<CreatePaymentRequest>(ContentType)
        //     .Produces<PaymentIntent>(200)
        //     .Produces(400);
                
        app.MapPost(ShopRoutes.Orders.CreateSession, CreatePaymentSession)
            .WithTags(Tags)
            .WithDescription("Create a new payment session")
            // .RequireAuthorization("Authenticated")
            .Produces<Session>(200)
            .Produces(400);
        
        return app;
    }

    // private static async Task<IResult> CreatePaymentIntent(
    //     HttpContext context,
    //     IOrderService orderService,
    //     CreatePaymentRequest createPaymentRequest,
    //     long orderId,
    //     CancellationToken cancellationToken)
    // {
    //     var userId = context.GetUserStripeIdFromClaims();
    //     var paymentIntent = await orderService.CreatePaymentAsync(orderId, userId, createPaymentRequest, cancellationToken);
    //     return Results.Ok(paymentIntent);
    // }
    private static async Task<IResult> CreatePaymentSession(
        HttpContext context,
        IOrderService orderService,
        long orderId,
        CancellationToken cancellationToken)
    {
        var userId = context.GetUserStripeIdFromClaims();
        // var userId = "cus_RhlWjl7AUz7B06";
        var paymentIntent = await orderService.CreateSessionAsync(orderId, userId, cancellationToken);
        return Results.Ok(paymentIntent);
    }
}