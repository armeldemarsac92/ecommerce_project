using MassTransit;
using Stripe;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.Messaging;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Consumer;

public class UpdateNutrimentsConsumer : IConsumer<UpdateNutrimentTask>
{
    private readonly IProductsService _productService;
    private readonly IOpenFoodFactService _openFoodFactService;
    private readonly ILogger<UpdateNutrimentsConsumer> _logger;

    public UpdateNutrimentsConsumer(
        ILogger<UpdateNutrimentsConsumer> logger,
        IProductsService productService, 
        IOpenFoodFactService openFoodFactService)
    {
        _logger = logger;
        _productService = productService;
        _openFoodFactService = openFoodFactService;
    }

    public async Task Consume(ConsumeContext<UpdateNutrimentTask> context)
    {
        var message = context.Message.Product;

        try
        {
            _logger.LogInformation("Checking if product {ProductId} needs to be updated.", message.Id);

            if (message.UpdatedAt.AddHours(1) > DateTime.UtcNow)
            {
                _logger.LogInformation("Product {ProductId} is up to date.", message.Id);
                return;
            }
            
            _logger.LogInformation("Updating product {ProductId} from Open Food Fact.", message.Id);
            var openFoodFactProduct = await _openFoodFactService.GetProductByBarCodeAsync(message.OpenFoodFactId, context.CancellationToken);
            if (openFoodFactProduct is null)
            {
                _logger.LogError("Product {ProductId} not found on Open Food Fact.", message.Id);
                return;
            }
            
            var updateNutrimentRequest = message.MapNutriments(openFoodFactProduct);
            if (updateNutrimentRequest is null)
            {
                _logger.LogInformation("No update needed for product {ProductId}.", message.Id);
                return;
            }
            
            await _productService.UpdateNutrimentsAsync(updateNutrimentRequest, context.CancellationToken);
            _logger.LogInformation("Updated product {ProductId} from Open Food Fact.", message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update product {ProductId}: {message}", message.Id, ex.Message);
            throw;
        }
    }
}
    
