using MassTransit;
using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.Messaging;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Stripe.SDK.Services;
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
        StripeConfiguration configuration,
        IOrderService orderService,
        IStripeChargeService chargeService,
        IPublishEndpoint publishEndpoint,
        CancellationToken cancellationToken)
    {
        var signingSecret = configuration.PaymentWebhookSecret;
            
        var stripeEvent = EventUtility.ConstructEvent(
            await new StreamReader(context.Request.Body).ReadToEndAsync(),
            context.Request.Headers["Stripe-Signature"],
            signingSecret
        );

        if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
        {
            var order = await orderService.GetOrderByPaymentIntentIdAsync(paymentIntent.Id, cancellationToken);

            switch (stripeEvent.Type)
            {
                case("payment_intent.succeeded"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "succeeded" }, cancellationToken);
                    await publishEndpoint.Publish(new CreateInvoiceTask() { order = order });
                    break;
                
                case("payment_intent.processing"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "processing" }, cancellationToken);
                    break;

                case("payment_intent.payment_failed"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "payment_failed" }, cancellationToken);
                    break;

                case("payment_intent.canceled"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "canceled" }, cancellationToken);
                    break;
            }
        }

        
        return Results.NoContent();
    }
}