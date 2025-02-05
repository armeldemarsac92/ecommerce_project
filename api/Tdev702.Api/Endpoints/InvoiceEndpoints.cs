using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Api.Utils;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class InvoiceEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Invoices";
    
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Orders.GetInvoice, GetInvoice)
            .WithTags(Tags)
            .WithDescription("Get an invoice by its parent order id.")
            .RequireAuthorization("Authenticated")
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetInvoice(
        HttpContext context,
        IOrderService orderService,
        long orderId,
        CancellationToken cancellationToken)
    {
        var userId = context.GetUserIdFromClaims();
        var role = context.GetUserRoleFromClaims();

        if (role != "Admin")
        {
            var order = await orderService.GetByIdAsync(orderId, cancellationToken);
            if (order.UserId != userId) return Results.Unauthorized();
        }
        var invoice = await orderService.GetOrderInvoice(orderId, cancellationToken);
            
        return Results.Ok(invoice);
    }
}