using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using StripeConfiguration = Tdev702.Contracts.Config.StripeConfiguration;

namespace Tdev702.Api.Endpoints;

public static class WebhookEndpoint
{
    private const string ContentType = "application/json";
    private const string Tags = "Webhooks";

    public static IEndpointRouteBuilder MapWebhookEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ShopRoutes.Webhooks.PaymentIntent, HandlePaymentEvent)
            .WithTags(Tags)
            .WithDescription("Update payment status for a payment intent")
            .Produces(204)
            .Produces(400);
        
        return app;
    }

    private static async Task<IResult> HandlePaymentEvent(
        HttpContext context,
        IConfiguration configuration,
        IOrderService orderService,
        CancellationToken cancellationToken)
    {
        var stripeConfiguration = configuration.GetSection("stripe").Get<StripeConfiguration>() ?? throw new InvalidOperationException("Stripe configuration not found");
        var signingSecret = stripeConfiguration.PaymentWebhookSecret;
            
        var stripeEvent = EventUtility.ConstructEvent(
            await new StreamReader(context.Request.Body).ReadToEndAsync(),
            context.Request.Headers["Stripe-Signature"],
            signingSecret
        );

        switch (stripeEvent.Type)
        {
            case("payment_intent.created"):
                await orderService.UpdateAsync()
            case("payment_intent.succeeded"):
            case("payment_intent.payment_failed"):
            case("payment_intent.canceled"):
                
        }
        return Results.NoContent();
    }
}