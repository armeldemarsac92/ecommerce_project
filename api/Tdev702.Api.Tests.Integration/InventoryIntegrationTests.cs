using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Request.Product;

namespace Tdev702.Api.Tests.Integration;

public class InventoryIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IInventoryEndpoints _inventoryApi;
    private readonly IProductEndpoints _productApi;
    
    public InventoryIntegrationTests(TestFixture fixture)
    {
        _inventoryApi = fixture.ServiceProvider.GetRequiredService<IInventoryEndpoints>();
        _productApi = fixture.ServiceProvider.GetRequiredService<IProductEndpoints>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnInventories()
    {
        // Act
        var response = await _inventoryApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenInventoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentInventoryId = 999999;
        
        // Act
        var response = await _inventoryApi.GetByIdAsync(nonExistentInventoryId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateInventory()
    {
        // Arrange
        // 1. First create a product
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // 2. Create inventory for this product
        var createRequest = new CreateInventoryRequest
        {
            ProductId = productId,
            Quantity = 100,
            Sku = $"SKU-{Guid.NewGuid()}"
        };
        
        // Act
        var response = await _inventoryApi.CreateAsync(createRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.ProductId.Should().Be(createRequest.ProductId);
        response.Content.Quantity.Should().Be(createRequest.Quantity);
        response.Content.Sku.Should().Be(createRequest.Sku);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenInventoryExists_ShouldUpdateInventory()
    {
        // Arrange
        // 1. First create a product
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // 2. Create inventory for this product
        var createRequest = new CreateInventoryRequest
        {
            ProductId = productId,
            Quantity = 100,
            Sku = $"SKU-{Guid.NewGuid()}"
        };
        
        var createResponse = await _inventoryApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var inventoryId = createResponse.Content!.Id;
        
        // 3. Prepare update
        var updateRequest = new UpdateInventoryRequest
        {
            Quantity = 150,
            Sku = $"SKU-UPDATED-{Guid.NewGuid()}"
        };
        
        // Act
        var updateResponse = await _inventoryApi.UpdateAsync(inventoryId, updateRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        updateResponse.Content!.Quantity.Should().Be(updateRequest.Quantity);
        updateResponse.Content.Sku.Should().Be(updateRequest.Sku);
    }
    
    [Fact]
    public async Task GetByProductIdAsync_WhenInventoryExists_ShouldReturnInventory()
    {
        // Arrange
        // 1. First create a product
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg",
            Quantity = 10
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // Act
        var response = await _inventoryApi.GetByProductIdAsync(productId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.ProductId.Should().Be(productId);
    }
    
    [Fact]
    public async Task IncrementStockAsync_WhenInventoryExists_ShouldIncrementStock()
    {
        // Arrange
        // 1. First create a product
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // 2. Create inventory for this product
        var createRequest = new CreateInventoryRequest
        {
            ProductId = productId,
            Quantity = 100,
            Sku = $"SKU-{Guid.NewGuid()}"
        };
        
        var createResponse = await _inventoryApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var initialQuantity = createResponse.Content!.Quantity;
        
        // 3. Prepare increment
        var incrementRequest = new UpdateQuantityRequest
        {
            Quantity = 50
        };
        
        // Act
        var incrementResponse = await _inventoryApi.IncrementStockAsync(productId, incrementRequest);
        
        // Assert
        incrementResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify the inventory was incremented
        var getResponse = await _inventoryApi.GetByProductIdAsync(productId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content!.Quantity.Should().Be(initialQuantity + incrementRequest.Quantity);
    }
    
    [Fact]
    public async Task DecrementStockAsync_WhenInventoryExists_ShouldDecrementStock()
    {
        // Arrange
        // 1. First create a product
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // 2. Create inventory for this product with sufficient stock
        var createRequest = new CreateInventoryRequest
        {
            ProductId = productId,
            Quantity = 100,
            Sku = $"SKU-{Guid.NewGuid()}"
        };
        
        var createResponse = await _inventoryApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var initialQuantity = createResponse.Content!.Quantity;
        
        // 3. Prepare decrement
        var decrementRequest = new UpdateQuantityRequest
        {
            Quantity = 30
        };
        
        // Act
        var decrementResponse = await _inventoryApi.DecrementStockAsync(productId, decrementRequest);
        
        // Assert
        decrementResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify the inventory was decremented
        var getResponse = await _inventoryApi.GetByProductIdAsync(productId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content!.Quantity.Should().Be(initialQuantity - decrementRequest.Quantity);
    }
}