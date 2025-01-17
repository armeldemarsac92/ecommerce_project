using Stripe;

namespace Tdev702.Stripe.SDK.Services;
public interface IStripeInvoiceService
{
    Task<Invoice> GetAsync(string id, InvoiceGetOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<Invoice> CreateAsync(InvoiceCreateOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<Invoice> UpdateAsync(string id, InvoiceUpdateOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<Invoice> DeleteAsync(string id, InvoiceDeleteOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
}
public class StripeInvoiceService : IStripeInvoiceService
{
    private readonly InvoiceService _invoiceService;

    public StripeInvoiceService(InvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }
    
    
    public async Task<Invoice> GetAsync(
        string id, 
        InvoiceGetOptions? options = null, 
        RequestOptions? requestOptions = null, 
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await _invoiceService.GetAsync(id, options, requestOptions, cancellationToken);
    }

    public async Task<Invoice> CreateAsync(
        InvoiceCreateOptions options, 
        RequestOptions? requestOptions = null, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        return await _invoiceService.CreateAsync(options, requestOptions, cancellationToken);
    }

    public async Task<Invoice> UpdateAsync(
        string id, 
        InvoiceUpdateOptions options, 
        RequestOptions? requestOptions = null, 
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(options);
        return await _invoiceService.UpdateAsync(id, options, requestOptions, cancellationToken);
    }

    public async Task<Invoice> DeleteAsync(
        string id, 
        InvoiceDeleteOptions? options = null, 
        RequestOptions? requestOptions = null, 
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await _invoiceService.DeleteAsync(id, options, requestOptions, cancellationToken);
    }
}
    
