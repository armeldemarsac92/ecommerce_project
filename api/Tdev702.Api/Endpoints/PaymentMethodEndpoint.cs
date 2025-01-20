using Microsoft.AspNetCore.Identity;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.Request.Shop.Payment;
using Tdev702.Contracts.Response.Shop;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class PaymentMethodEndpoint
{
    private const string ContentType = "application/json";
    private const string Tags = "Payment Methods";

    public static IEndpointRouteBuilder MapPaymentMethodEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.PaymentMethods.Get, GetPaymentMethods)
            .WithTags(Tags)
            .WithDescription("Get all payment methods")
            .RequireAuthorization("Authenticated")
            .Produces<List<PaymentMethodResponse>>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.PaymentMethods.Attach, AttachPaymentMethod)
            .WithTags(Tags)
            .WithDescription("Attach a payment method to a customer")
            .RequireAuthorization("Authenticated")
            .Produces(204)
            .Produces(400);
        
        app.MapPost(ShopRoutes.PaymentMethods.Attach, DetachPaymentMethod)
            .WithTags(Tags)
            .WithDescription("Detach a payment method from a customer")
            .RequireAuthorization("Authenticated")
            .Produces(204)
            .Produces(400);
        
        return app;
    }

    private static async Task<IResult> DetachPaymentMethod(
        HttpContext context,
        IStripePaymentMethodService stripePaymentMethodService,
        UserManager<User> userManager,
        string paymentMethodId,
        CancellationToken cancellationToken)
    {
        await stripePaymentMethodService.DetachAsync(paymentMethodId, null, null, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> AttachPaymentMethod(
        HttpContext context,
        IStripePaymentMethodService stripePaymentMethodService,
        UserManager<User> userManager,
        string paymentMethodId,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user == null) throw new BadRequestException("Unknown user");
        if (user.StripeCustomerId == null) throw new BadRequestException("User does not have a Stripe customer ID");
        await stripePaymentMethodService.AttachAsync(paymentMethodId, new PaymentMethodAttachOptions { Customer = user.StripeCustomerId }, null, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> GetPaymentMethods(
        HttpContext context,
        IStripeCustomerPaymentMethodService stripePaymentMethodService,
        UserManager<User> userManager,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(context.User);
        if (user == null) throw new BadRequestException("Unknown user");
        if (user.StripeCustomerId == null) throw new BadRequestException("User does not have a Stripe customer ID");
        var paymentMethods = await stripePaymentMethodService.ListAsync(user.StripeCustomerId, null, null, cancellationToken);
        return Results.Ok(paymentMethods.MapToPaymentMethods());
    }
}