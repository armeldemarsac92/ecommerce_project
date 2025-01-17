using Stripe;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Stripe.SDK.Services;

public interface IStripePaymentMethodService
{
    Task<PaymentMethod> CreateAsync(PaymentMethodCreateOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<PaymentMethod> GetAsync(string paymentMethodId, PaymentMethodGetOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<PaymentMethod> UpdateAsync(string paymentMethodId, PaymentMethodUpdateOptions? options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<PaymentMethod> AttachAsync(string paymentMethodId, PaymentMethodAttachOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<PaymentMethod> DetachAsync(string paymentMethodId, PaymentMethodDetachOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
}

public class StripePaymentMethodService : IStripePaymentMethodService
{
    private readonly PaymentMethodService _paymentMethodService;

    public StripePaymentMethodService(PaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    public async Task<PaymentMethod> CreateAsync(PaymentMethodCreateOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        return await _paymentMethodService.CreateAsync(options, requestOptions, cancellationToken);
    }

    public async Task<PaymentMethod> GetAsync(string paymentMethodId, PaymentMethodGetOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(paymentMethodId))
        {
            throw new BadRequestException(nameof(paymentMethodId));
        }

        return await _paymentMethodService.GetAsync(paymentMethodId, options, requestOptions, cancellationToken);
    }

    public async Task<PaymentMethod> UpdateAsync(string paymentMethodId, PaymentMethodUpdateOptions? options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(paymentMethodId))
        {
            throw new BadRequestException(nameof(paymentMethodId));
        }

        return await _paymentMethodService.UpdateAsync(paymentMethodId, options, requestOptions, cancellationToken);
    }

    public async Task<PaymentMethod> AttachAsync(string paymentMethodId, PaymentMethodAttachOptions options, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(paymentMethodId))
        {
            throw new BadRequestException(nameof(paymentMethodId));
        }

        return await _paymentMethodService.AttachAsync(paymentMethodId, options, requestOptions, cancellationToken);
    }

    public async Task<PaymentMethod> DetachAsync(string paymentMethodId, PaymentMethodDetachOptions? options = null, RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(paymentMethodId))
        {
            throw new BadRequestException(nameof(paymentMethodId));
        }

        return await _paymentMethodService.DetachAsync(paymentMethodId, options, requestOptions, cancellationToken);
    }
}