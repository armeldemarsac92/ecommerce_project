using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Endpoints;

public static class InvoiceEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Invoices";
    
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ShopRoutes.Invoices.GetById, GetInvoice)
            .WithTags(Tags)
            .WithDescription("Get an invoice by ID")
            .Produces<Invoice>(200)
            .Produces(404);
        
        app.MapPost(ShopRoutes.Invoices.Create, CreateInvoice)
            .WithTags(Tags)
            .WithDescription("Create a new invoice")
            .Accepts<InvoiceCreateOptions>(ContentType)
            .Produces<Invoice>(200)
            .Produces(400);
        
        app.MapPut(ShopRoutes.Invoices.Update, UpdateInvoice)
            .WithTags(Tags)
            .WithDescription("Update an existing invoice")
            .Accepts<InvoiceUpdateOptions>(ContentType)
            .Produces<Invoice>(200)
            .Produces(404);
        
        app.MapDelete(ShopRoutes.Invoices.Delete, DeleteInvoice)
            .WithTags(Tags)
            .WithDescription("Delete an invoice")
            .Produces(204)
            .Produces(404);
        
        return app;
    }
    
    private static async Task<IResult> GetInvoice(
        HttpContext context,
        IStripeInvoiceService stripeInvoiceService,
        string id,
        CancellationToken cancellationToken)
    {   
        var invoice = await stripeInvoiceService.GetAsync(
            id,
            null, 
            null,  
            cancellationToken);
            
        return Results.Ok(invoice);
    }
    
    private static async Task<IResult> CreateInvoice(
        HttpContext context,
        IStripeInvoiceService stripeInvoiceService,
        InvoiceCreateOptions invoiceOptions,
        CancellationToken cancellationToken)
    {
        var invoice = await stripeInvoiceService.CreateAsync(
            invoiceOptions,
            null, 
            cancellationToken);
            
        return Results.Ok(invoice);
    }

    private static async Task<IResult> UpdateInvoice(
        HttpContext context,
        IStripeInvoiceService stripeInvoiceService,
        string id,
        InvoiceUpdateOptions invoiceOptions,
        CancellationToken cancellationToken)
    {
        var invoice = await stripeInvoiceService.UpdateAsync(
            id,
            invoiceOptions,
            null, 
            cancellationToken);
            
        return Results.Ok(invoice);
    }

    private static async Task<IResult> DeleteInvoice(
        HttpContext context,
        IStripeInvoiceService stripeInvoiceService,
        string id,
        CancellationToken cancellationToken)
    {
        await stripeInvoiceService.DeleteAsync(
            id,
            null, // options
            null, // requestOptions
            cancellationToken);
            
        return Results.NoContent();
    }
}