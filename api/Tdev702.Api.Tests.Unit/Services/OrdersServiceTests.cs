using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Stripe;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;
using Tdev702.Stripe.SDK.Services;

public class OrderServiceTests
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IStripePaymentIntentService _stripePaymentIntentService;
    private readonly IStripeInvoiceService _stripeInvoicesService;
    private readonly IStripeSessionService _stripeSessionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _logger = Substitute.For<ILogger<OrderService>>();
        _orderRepository = Substitute.For<IOrderRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _orderProductRepository = Substitute.For<IOrderProductRepository>();
        _inventoryRepository = Substitute.For<IInventoryRepository>();
        _stripePaymentIntentService = Substitute.For<IStripePaymentIntentService>();
        _stripeInvoicesService = Substitute.For<IStripeInvoiceService>();
        _stripeSessionService = Substitute.For<IStripeSessionService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _sut = new OrderService(
            _logger,
            _orderRepository,
            _productRepository,
            _orderProductRepository,
            _stripePaymentIntentService,
            _unitOfWork,
            _inventoryRepository,
            _stripeInvoicesService,
            _stripeSessionService
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenOrderExists_ShouldReturnOrderSummary()
    {
        // Arrange
        var orderId = 1;
        var userId = new Guid().ToString();
        var sqlResponse = new OrderSummarySQLResponse { Id = orderId, UserId = userId, TotalAmount = 100, StripePaymentStatus = "unpaid", StripeSessionStatus = "draft"};
        var expectedResponse = sqlResponse.MapToOrderSummary();

        _orderRepository.GetByIdAsync(orderId, default)
            .Returns(sqlResponse);

        // Act
        var result = await _sut.GetByIdAsync(orderId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var orderId = 1;
        _orderRepository.GetByIdAsync(orderId, default)
            .Returns((OrderSummarySQLResponse)null);

        // Act
        var act = () => _sut.GetByIdAsync(orderId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Order {orderId} not found");
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldReturnCreatedOrder()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var sessionId = "session_123";
        var now = DateTime.UtcNow;
        
        var createRequest = new CreateOrderRequest 
        { 
            UserId = userId,
            Products = new List<CreateOrderProductRequest> 
            { 
                new() { ProductId = 1, Quantity = 2 }, 
                new() { ProductId = 2, Quantity = 2 } 
            } 
        };

        var products = new List<FullProductSQLResponse> 
        { 
            new() 
            { 
                Id = 1, 
                Price = 50.00,
                Title = "Test Product 1",
                Description = "Test Description 1",
                ImageUrl = "http://test.com/image1.jpg",
                UpdatedAt = now,
                CreatedAt = now,
                Tags = new[] { "tag1", "tag2" },
                BrandTitle = "Test Brand 1",
                CategoryTitle = "Test Category 1",
                OpenFoodFactId = "123456"
            },
            new() 
            { 
                Id = 2, 
                Price = 75.00,
                Title = "Test Product 2",
                Description = "Test Description 2",
                ImageUrl = "http://test.com/image2.jpg",
                UpdatedAt = now,
                CreatedAt = now,
                Tags = new[] { "tag2", "tag3" },
                BrandTitle = "Test Brand 2",
                CategoryTitle = "Test Category 2",
                OpenFoodFactId = "789012"
            }
        };

        var createdOrderId = 1;
        var orderResponse = new OrderSummarySQLResponse 
        { 
            Id = 1,
            UserId = userId,
            StripeSessionId = sessionId,
            StripePaymentStatus = "no_payment_required", 
            StripeSessionStatus = "draft", 
            TotalAmount = 250.00, 
            CreatedAt = now,
            StripeInvoiceId = null,
            OrderItems = new OrderItem[] 
            {
                new() 
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 50.00,
                    Title = "Test Product 1",
                    Description = "Test Description 1",
                    Picture = "http://test.com/image1.jpg",
                    Subtotal = 100.00,
                    Brand = "Test Brand 1", 
                    Category = "Test Category 1"  
                },
                new() 
                {
                    ProductId = 2,
                    Quantity = 2, 
                    UnitPrice = 75.00,
                    Title = "Test Product 2",
                    Description = "Test Description 2",
                    Picture = "http://test.com/image2.jpg",
                    Subtotal = 150.00, // Updated for quantity of 2
                    Brand = "Test Brand 2", 
                    Category = "Test Category 2"  
                }
            }
        };
        
        _productRepository.GetByIdsAsync(Arg.Is<List<long>>(x => x.SequenceEqual(new List<long> { 1, 2 })), default)
            .Returns(products);
            
        _orderRepository.CreateAsync(Arg.Any<CreateOrderSQLRequest>(), default)
            .Returns(createdOrderId);
            
        _orderRepository.GetByIdAsync(createdOrderId, default)
            .Returns(orderResponse);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().BeEquivalentTo(orderResponse.MapToOrderSummary());
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task GetOrderByPaymentIntentIdAsync_WhenExists_ShouldReturnOrder()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var sessionId = "session_123";
        var now = DateTime.UtcNow;
        var orderResponse = new OrderSummarySQLResponse 
        { 
            Id = 1,
            UserId = userId,
            StripeSessionId = sessionId,
            StripePaymentStatus = "pending",
            StripeSessionStatus = "open",
            TotalAmount = 100.00,
            CreatedAt = now,
            StripeInvoiceId = null,
            OrderItems = new OrderItem[] 
            {
                new() 
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 50.00,
                    Title = "Test Product",
                    Description = "Test Description",
                    Picture = "http://test.com/image.jpg",
                    Subtotal = 100.00,  
                    Brand = "Test Brand", 
                    Category = "Test Category"  
                }
            }
        };

        _orderRepository.GetBySessionIdAsync(sessionId, default)
            .Returns(orderResponse);

        // Act
        var result = await _sut.GetOrderByPaymentIntentIdAsync(sessionId, default);

        // Assert
        result.Should().BeEquivalentTo(orderResponse.MapToOrderSummary());
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_ShouldDeleteOrder()
    {
        // Arrange
        var orderId = 1L;
        _orderRepository.DeleteAsync(orderId, default)
            .Returns(1);

        // Act
        await _sut.DeleteAsync(orderId);

        // Assert
        await _orderRepository.Received(1).DeleteAsync(orderId, default);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }
}