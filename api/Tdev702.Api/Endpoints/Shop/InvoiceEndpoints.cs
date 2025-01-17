using Stripe;
using Tdev702.Api.Routes;
using Tdev702.Api.Services;
using Tdev702.Contracts.SQL.Request.Shop.Product;

namespace Tdev702.Api.Endpoints.Shop;

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
            .Produces<Invoice>(200)
            .Produces(400);
        
        app.MapPut(ShopRoutes.Invoices.Update, UpdateInvoice)
            .WithTags(Tags)
            .WithDescription("Update an existing invoice")
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
        IInvoicesService invoicesService,
        string id,
        CancellationToken cancellationToken)
    {   
        try 
        {
            var invoice = await invoicesService.GetAsync(
                id,
                null,  // options
                null,  // requestOptions
                cancellationToken);
                
            return Results.Ok(invoice);
        }
        catch (StripeException ex) when (ex.StripeError.Type == "invalid_request_error")
        {
            return Results.NotFound();
        }
    }
    
    private static async Task<IResult> CreateInvoice(
        HttpContext context,
        IInvoicesService invoicesService,
        InvoiceCreateOptions invoiceOptions,
        CancellationToken cancellationToken)
    {
        try 
        {
            var invoice = await invoicesService.CreateAsync(
                invoiceOptions,
                null, // requestOptions
                cancellationToken);
                
            return Results.Ok(invoice);
        }
        catch (StripeException ex)
        {
            return Results.BadRequest(new { error = ex.StripeError.Message });
        }
    }

    private static async Task<IResult> UpdateInvoice(
        HttpContext context,
        IInvoicesService invoicesService,
        string id,
        InvoiceUpdateOptions invoiceOptions,
        CancellationToken cancellationToken)
    {
        try 
        {
            var invoice = await invoicesService.UpdateAsync(
                id,
                invoiceOptions,
                null, // requestOptions
                cancellationToken);
                
            return Results.Ok(invoice);
        }
        catch (StripeException ex) when (ex.StripeError.Type == "invalid_request_error")
        {
            return Results.NotFound();
        }
        catch (StripeException ex)
        {
            return Results.BadRequest(new { error = ex.StripeError.Message });
        }
    }

    private static async Task<IResult> DeleteInvoice(
        HttpContext context,
        IInvoicesService invoicesService,
        string id,
        CancellationToken cancellationToken)
    {
        try 
        {
            await invoicesService.DeleteAsync(
                id,
                null, // options
                null, // requestOptions
                cancellationToken);
                
            return Results.NoContent();
        }
        catch (StripeException ex) when (ex.StripeError.Type == "invalid_request_error")
        {
            return Results.NotFound();
        }
    }
}