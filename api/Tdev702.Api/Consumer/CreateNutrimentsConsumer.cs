using MassTransit;
using Stripe;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Consumer;

public class CreateNutrimentsConsumer : IConsumer<FullProductSQLResponse>
{
    private readonly IProductsService _productService;
    private readonly IOpenFoodFactService _openFoodFactService;
    private readonly ILogger<OrderStockConsumer> _logger;

    public CreateNutrimentsConsumer(
        ILogger<OrderStockConsumer> logger,
        IProductsService productService, 
        IOpenFoodFactService openFoodFactService)
    {
        _logger = logger;
        _productService = productService;
        _openFoodFactService = openFoodFactService;
    }

    public async Task Consume(ConsumeContext<FullProductSQLResponse> context)
    {
        var message = context.Message;

        try
        {
            _logger.LogInformation("Updating product {ProductId} with Open Food Fact Data", message.Id);
           _logger.LogInformation("Creating nutriments for product {ProductId}", message.Id);
           
           var openFoodFactProduct = await _openFoodFactService.GetProductByBarCodeAsync(message.OpenFoodFactId);
           if (openFoodFactProduct is null)
           {
               _logger.LogError("Product {ProductId} not found on Open Food Fact.", message.Id);
               return;
           }
           
           var createNutrimentRequest = openFoodFactProduct.Nutriments.MapToCreateNutrimentSQLRequest(message.Id);
           await _productService.CreateNutrimentsAsync(createNutrimentRequest);
           _logger.LogInformation("Nutriments created for product {ProductId}", message.Id);

           _logger.LogInformation("Adding picture to product {ProductId}", message.Id);
           await _productService.AddPictureAsync(message.Id, openFoodFactProduct.ImageUrl);
           _logger.LogInformation("Picture added to product {ProductId}", message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError( "Failed to create nutriments for product {ProductId}: {message}", message.Id, ex.Message);
            throw;
        }
    }
}
    
