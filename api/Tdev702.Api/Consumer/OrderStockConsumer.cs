using MassTransit;
using Stripe;
using Tdev702.Contracts.API.Response;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Consumer;

public class OrderStockConsumer
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderStockConsumer> _logger;

    public OrderStockConsumer(
        ILogger<OrderStockConsumer> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<OrderResponse> context)
    {
        var message = context.Message;
        
        try
        {
            _logger.LogInformation("Updating stock for order {OrderId}", message.Id);
            
            foreach (var orderProduct in message.Products)
            {
                var product = await _productRepository.GetByIdAsync(orderProduct.Product.Id, CancellationToken.None);
                if (product is null)
                {
                    _logger.LogError("Product not found for order product {OrderProductId}", orderProduct.Id);
                    continue;
                }
                
                product.Stock -= orderProduct.Quantity;
                await _productRepository.UpdateAsync(product, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            throw; 
        }
    }
}