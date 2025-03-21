using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Request.Product;

namespace Tdev702.Api.Tests.Integration;

public class OrderIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IOrderEndpoints _orderApi;
    private readonly IProductEndpoints _productApi;
    
    public OrderIntegrationTests(TestFixture fixture)
    {
        _orderApi = fixture.ServiceProvider.GetRequiredService<IOrderEndpoints>();
        _productApi = fixture.ServiceProvider.GetRequiredService<IProductEndpoints>();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnOrders()
    {
        // Act
        var response = await _orderApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentOrderId = 999999;
        
        // Act
        var response = await _orderApi.GetByIdAsync(nonExistentOrderId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetUserOrdersAsync_ShouldReturnUserOrders()
    {
        // Act
        var response = await _orderApi.GetUserOrdersAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateOrder()
    {
        // Arrange
        // 1. Create a product first
        var productRequest = new CreateProductRequest
        {
            Title = $"Test Product for Order {Guid.NewGuid()}",
            Description = "For order creation test",
            Price = 49.99,
            ImageUrl = "https://example.com/order-product.jpg"
        };
        
        var productResponse = await _productApi.CreateAsync(productRequest);
        productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = productResponse.Content!.Id;
        
        // 2. Prepare order request
        var orderRequest = new CreateOrderRequest
        {
            Products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = productId,
                    Quantity = 2
                }
            }
        };
        
        // Act
        var orderResponse = await _orderApi.CreateAsync(orderRequest);
        
        // Assert
        orderResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        orderResponse.Content.Should().NotBeNull();
        orderResponse.Content.OrderItems.Should().NotBeNull();
        orderResponse.Content.OrderItems.Should().HaveCount(1);
        orderResponse.Content.OrderItems[0].ProductId.Should().Be(productId);
        orderResponse.Content.OrderItems[0].Quantity.Should().Be(2);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenOrderExists_ShouldUpdateOrder()
    {
        // Arrange
        // 1. Create products for the order
        var productRequest1 = new CreateProductRequest
        {
            Title = $"Test Product 1 for Order Update {Guid.NewGuid()}",
            Description = "For order update test",
            Price = 29.99,
            ImageUrl = "https://example.com/order-update-1.jpg",
            Quantity = 10
        };
        
        var productResponse1 = await _productApi.CreateAsync(productRequest1);
        productResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId1 = productResponse1.Content!.Id;
        
        var productRequest2 = new CreateProductRequest
        {
            Title = $"Test Product 2 for Order Update {Guid.NewGuid()}",
            Description = "For order update test",
            Price = 39.99,
            ImageUrl = "https://example.com/order-update-2.jpg",
            Quantity = 10
        };
        
        var productResponse2 = await _productApi.CreateAsync(productRequest2);
        productResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId2 = productResponse2.Content!.Id;
        
        // 2. Create an order with the first product
        var createOrderRequest = new CreateOrderRequest
        {
            Products = new List<OrderProduct>
            {
                new()
                {
                    ProductId = productId1,
                    Quantity = 1
                }
            }
        };
        
        var createOrderResponse = await _orderApi.CreateAsync(createOrderRequest);
        createOrderResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var orderId = createOrderResponse.Content!.Id;
        
        // 3. Prepare update request - changing product and quantity
        var updateOrderRequest = new UpdateOrderRequest
        {
            Products = new List<OrderProduct>
            {
                new()
                {
                    ProductId = productId2,
                    Quantity = 3
                }
            }
        };
        
        // Act
        var updateResponse = await _orderApi.UpdateAsync(orderId, updateOrderRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        
        // Products should have been updated
        updateResponse.Content!.OrderItems.Should().HaveCount(1);
        updateResponse.Content.OrderItems[0].ProductId.Should().Be(productId2);
        updateResponse.Content.OrderItems[0].Quantity.Should().Be(3);
        updateResponse.Content.TotalAmount.Should().Be(39.99 * 3);
    }    
    
    [Fact]
    public async Task UpdateAsync_WhenOrderExists_ShouldUpdateOrderAndSetTotalToZero()
    {
        // Arrange
        // 1. Create products for the order
        var productRequest1 = new CreateProductRequest
        {
            Title = $"Test Product 1 for Order Update {Guid.NewGuid()}",
            Description = "For order update test",
            Price = 29.99,
            ImageUrl = "https://example.com/order-update-1.jpg",
            Quantity = 10
        };
        
        var productResponse1 = await _productApi.CreateAsync(productRequest1);
        productResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId1 = productResponse1.Content!.Id;
        
        // 2. Create an order with the first product
        var createOrderRequest = new CreateOrderRequest
        {
            Products = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = productId1,
                    Quantity = 1
                }
            }
        };
        
        var createOrderResponse = await _orderApi.CreateAsync(createOrderRequest);
        createOrderResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var orderId = createOrderResponse.Content!.Id;
        
        // 3. Prepare update request - changing product and quantity
        var updateOrderRequest = new UpdateOrderRequest
        {
            Products = new List<OrderProduct>
            {
            }
        };
        
        // Act
        var updateResponse = await _orderApi.UpdateAsync(orderId, updateOrderRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        
        // Products should have been updated
        updateResponse.Content!.OrderItems.Should().HaveCount(0);
        updateResponse.Content.TotalAmount.Should().Be(0);
    }
    
    // [Fact]
    // public async Task GetInvoiceAsync_WhenOrderExists_ShouldReturnInvoice()
    // {
    //     // Arrange
    //     // 1. Create a product for the order
    //     var productRequest = new CreateProductRequest
    //     {
    //         Title = $"Test Product for Invoice {Guid.NewGuid()}",
    //         Description = "For invoice test",
    //         Price = 59.99,
    //         ImageUrl = "https://example.com/invoice-product.jpg"
    //     };
    //     
    //     var productResponse = await _productApi.CreateAsync(productRequest);
    //     productResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    //     var productId = productResponse.Content!.Id;
    //     
    //     // 2. Create an order with the product
    //     var createOrderRequest = new CreateOrderRequest
    //     {
    //         Products = new List<CreateOrderProductRequest>
    //         {
    //             new CreateOrderProductRequest
    //             {
    //                 ProductId = productId,
    //                 Quantity = 1
    //             }
    //         }
    //     };
    //     
    //     var createOrderResponse = await _orderApi.CreateAsync(createOrderRequest);
    //     createOrderResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    //     var orderId = createOrderResponse.Content!.Id;
    //     
    //     // Depending on the implementation, you might need to simulate payment processing
    //     // before an invoice is available. This is simplified for the test.
    //     
    //     // Act
    //     var invoiceResponse = await _orderApi.GetInvoiceAsync(orderId);
    //     
    //     // Assert
    //     // Assuming the invoice endpoint returns at least OK even if invoice generation is pending
    //     invoiceResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    // }
}