using MassTransit;
using Stripe;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Messaging;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Repository.Repository;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Consumer;

public class CreateInvoiceConsumer : IConsumer<CreateInvoiceTask>
{
    private readonly IStripeInvoiceService _stripeInvoiceService;
    private readonly ICustomerService _customerService;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderStockConsumer> _logger;

    public CreateInvoiceConsumer(
        ILogger<OrderStockConsumer> logger,
        IStripeInvoiceService stripeInvoiceService, 
        ICustomerService customerService, 
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _stripeInvoiceService = stripeInvoiceService;
        _customerService = customerService;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<CreateInvoiceTask> context)
    {
        var message = context.Message;
        var order = message.order;
        var customer = await _customerService.GetByIdAsync(order.UserId);

        try
        {
            _logger.LogInformation("Creating stripe invoice for order {orderId}.", order.Id);
            
            _logger.LogInformation("First creating invoice items.");
            foreach (var orderItem in order.OrderItems)
            {
                _logger.LogInformation("Creating invoice item for product {productId}.", orderItem.ProductId);
                await _stripeInvoiceService.CreateItemAsync(new InvoiceItemCreateOptions()
                {
                    Customer = customer.StripeId,
                    Currency = "eur",
                    Description = orderItem.Title,
                    Quantity = orderItem.Quantity,
                    UnitAmount = (long)orderItem.UnitPrice*100,
                    TaxRates = new List<string>(){"txr_1Qp8cJLx56XjlpN63vQ03n96"}
                });
                _logger.LogInformation("Invoice item created for product {productId}.", orderItem.ProductId);
            }
            
            _logger.LogInformation("Then creating the invoice.");
            var invoice = await _stripeInvoiceService.CreateAsync(new InvoiceCreateOptions()
            {
                Customer = customer.StripeId,
                Currency = "eur",
                Description = "Facture pour la commande no " + order.Id,
                PendingInvoiceItemsBehavior = "include"
            });
            await _stripeInvoiceService.PayAsync(invoice.Id, new InvoicePayOptions()
            {
                PaidOutOfBand = true
            });
            _logger.LogInformation("Invoice {invoiceId} created for order {orderId}.", invoice.Id, order.Id);
            
            _logger.LogInformation("Updating order with stripe invoice id.");
            await _orderRepository.UpdateAsync(new UpdateOrderSQLRequest()
                { Id = order.Id, StripeInvoiceId = invoice.Id });
            _logger.LogInformation("Order {orderId} updated with stripe invoice id successfully.", order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating invoice for order {orderId}.", order.Id);
        }
    }
}
    
