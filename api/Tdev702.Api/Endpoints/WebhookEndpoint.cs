using MassTransit;
using Stripe;
using Stripe.Checkout;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.Messaging;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Stripe.SDK.Services;
using Event = Stripe.Event;
using StripeConfiguration = Tdev702.Contracts.Config.StripeConfiguration;

namespace Tdev702.Api.Endpoints;

public static class WebhookEndpoint
{
    private const string ContentType = "application/json";
    private const string Tags = "Webhooks";

    public static IEndpointRouteBuilder MapWebhookEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ShopRoutes.Webhooks.Session, HandleSessionEvent)
            .WithTags(Tags)
            .WithDescription("Update payment status for a payment intent")
            .Produces(204)
            .Produces(400);        
        
        // app.MapPost(ShopRoutes.Webhooks.PaymentIntent, HandlePaymentEvent)
        //     .WithTags(Tags)
        //     .WithDescription("Update payment status for a payment intent")
        //     .Produces(204)
        //     .Produces(400);
        
        return app;
    }

    // private static async Task<IResult> HandlePaymentEvent(
    //     HttpContext context,
    //     StripeConfiguration configuration,
    //     IOrderService orderService,
    //     IStripeChargeService chargeService,
    //     IPublishEndpoint publishEndpoint,
    //     CancellationToken cancellationToken)
    // {
    //     var signingSecret = configuration.PaymentWebhookSecret;
    //         
    //     var stripeEvent = EventUtility.ConstructEvent(
    //         await new StreamReader(context.Request.Body).ReadToEndAsync(),
    //         context.Request.Headers["Stripe-Signature"],
    //         signingSecret
    //     );
    //
    //     if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
    //     {
    //         var order = await orderService.GetOrderByPaymentIntentIdAsync(paymentIntent.Id, cancellationToken);
    //
    //         switch (stripeEvent.Type)
    //         {
    //             case("payment_intent.succeeded"):
    //                 await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "succeeded" }, cancellationToken);
    //                 await publishEndpoint.Publish(new CreateInvoiceTask() { order = order });
    //                 break;
    //             
    //             case("payment_intent.processing"):
    //                 await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "processing" }, cancellationToken);
    //                 break;
    //
    //             case("payment_intent.payment_failed"):
    //                 await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "payment_failed" }, cancellationToken);
    //                 break;
    //
    //             case("payment_intent.canceled"):
    //                 await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, PaymentStatus = "canceled" }, cancellationToken);
    //                 break;
    //         }
    //     }
    //
    //     
    //     return Results.NoContent();
    // }
    
    private static async Task<IResult> HandleSessionEvent(
        HttpContext context,
        StripeConfiguration configuration,
        IOrderService orderService,
        IPublishEndpoint publishEndpoint,
        CancellationToken cancellationToken)
    {
        var signingSecret = configuration.PaymentWebhookSecret;
            
        var stripeEvent = EventUtility.ConstructEvent(
            await new StreamReader(context.Request.Body).ReadToEndAsync(),
            context.Request.Headers["Stripe-Signature"],
            signingSecret
        );

        if (stripeEvent.Data.Object is Session session)
        {
            var order = await orderService.GetByIdAsync(long.Parse(session.ClientReferenceId), cancellationToken);

            switch (stripeEvent.Type)
            {
                case("checkout.session.completed"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, StripeInvoiceId = session.InvoiceId, StripePaymentStatus = session.PaymentStatus, StripeSessionStatus = session.Status}, cancellationToken);
                    break;
                
                case("checkout.session.expired"):
                    await orderService.UpdateOrderPaymentStatus(new UpdateOrderSQLRequest { Id = order.Id, StripeSessionStatus = "expired" }, cancellationToken);
                    break;
            }
        }

        
        return Results.NoContent();
    }
}