using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Order;
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

        if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
        {
            var orderId = await orderService.GetOrderIdByPaymentIntentIdAsync(paymentIntent.Id, cancellationToken);

            switch (stripeEvent.Type)
            {
                case("payment_intent.created"):
                    await orderService.UpdateAsync(new UpdateOrderRequest { Id = orderId, PaymentStatus = "created" }, cancellationToken);
                    break;
                case("payment_intent.succeeded"):
                    await orderService.UpdateAsync(new UpdateOrderRequest { Id = orderId, PaymentStatus = "succeded" }, cancellationToken);
                    break;
                
                case("payment_intent.processing"):
                    await orderService.UpdateAsync(new UpdateOrderRequest { Id = orderId, PaymentStatus = "processing" }, cancellationToken);
                    break;

                case("payment_intent.payment_failed"):
                    await orderService.UpdateAsync(new UpdateOrderRequest { Id = orderId, PaymentStatus = "failed" }, cancellationToken);
                    break;

                case("payment_intent.canceled"):
                    await orderService.UpdateAsync(new UpdateOrderRequest { Id = orderId, PaymentStatus = "canceled" }, cancellationToken);
                    break;

                
            }
        }

        
        return Results.NoContent();
    }
}