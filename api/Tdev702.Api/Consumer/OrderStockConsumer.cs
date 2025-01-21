using MassTransit;
using Stripe;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Consumer;

public class OrderStockConsumer
{
    private readonly IInventoriesService _inventoriesService;
    private readonly IProductsService _productService;
    private readonly ILogger<OrderStockConsumer> _logger;

    public OrderStockConsumer(
        ILogger<OrderStockConsumer> logger,
        IInventoriesService inventoriesService,
        IProductsService productService)
    {
        _logger = logger;
        _inventoriesService = inventoriesService;
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<OrderResponse> context)
    {
        var message = context.Message;

        try
        {
            _logger.LogInformation("Freeing stock for order {OrderId}", message.Id);

            foreach (var orderProduct in message.Products)
            {
                await _inventoriesService.IncreamentAsync(orderProduct.Quantity, orderProduct.Product.Id);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
    
