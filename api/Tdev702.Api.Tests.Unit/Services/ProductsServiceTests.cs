using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.Messaging;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Request.ProductTag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

public class ProductsServiceTests
{
    private readonly IProductRepository _productRepository;
    private readonly IProductTagRepository _productTagRepository;
    private readonly INutrimentsRepository _nutrimentsRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsService> _logger;
    private readonly ProductsService _sut;

    public ProductsServiceTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _productTagRepository = Substitute.For<IProductTagRepository>();
        _nutrimentsRepository = Substitute.For<INutrimentsRepository>();
        _inventoryRepository = Substitute.For<IInventoryRepository>();
        _tagRepository = Substitute.For<ITagRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _logger = Substitute.For<ILogger<ProductsService>>();

        _sut = new ProductsService(
            _productRepository,
            _productTagRepository,
            _logger,
            _tagRepository,
            _unitOfWork,
            _nutrimentsRepository,
            _publishEndpoint,
            _inventoryRepository
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnProductAndPublishMessage()
    {
        // Arrange
        var productId = 1;
        var now = DateTime.UtcNow;
        var product = new FullProductSQLResponse
        {
            Id = productId,
            Title = "Test Product",
            Description = "Test Description",
            Price = 50.00,
            ImageUrl = "http://test.com/image.jpg",
            UpdatedAt = now,
            CreatedAt = now
        };

        _productRepository.GetByIdAsync(productId, default)
            .Returns(product);

        // Act
        var result = await _sut.GetByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
        await _publishEndpoint.Received(1).Publish(
            Arg.Is<UpdateNutrimentTask>(x => x.Product == product), 
            default);
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_WithTagsAndQuantity_ShouldReturnProduct()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var createRequest = new CreateProductRequest
        {
            Title = "New Product",
            Description = "New Description",
            Price = 50.00,
            ImageUrl = "http://test.com/image.jpg",
            TagIds = new List<long> { 1, 2 },
            Quantity = 10
        };

        var createdProductId = 1;
        var createdProduct = new FullProductSQLResponse
        {
            Id = createdProductId,
            Title = createRequest.Title,
            Description = createRequest.Description,
            Price = createRequest.Price,
            ImageUrl = createRequest.ImageUrl,
            UpdatedAt = now,
            CreatedAt = now
        };

        _productRepository.CreateAsync(Arg.Any<CreateProductSQLRequest>(), default)
            .Returns(createdProductId);
        _productRepository.GetByIdAsync(createdProductId, default)
            .Returns(createdProduct);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(createdProductId);
        
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _productTagRepository.Received(2).CreateAsync(Arg.Any<CreateProductTagSQLRequest>(), default);
        await _inventoryRepository.Received(1).CreateAsync(
            Arg.Is<CreateInventorySQLRequest>(x => 
                x.ProductId == createdProductId && 
                x.Quantity == createRequest.Quantity), 
            default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task UpdateAsync_WhenSuccessful_WithTagChanges_ShouldReturnUpdatedProduct()
    {
        // Arrange
        var productId = 1;
        var now = DateTime.UtcNow;
        var updateRequest = new UpdateProductRequest
        {
            Title = "Updated Product",
            Description = "Updated Description",
            Price = 75.00,
            ImageUrl = "http://test.com/updated.jpg",
            TagIds = new List<long> { 2, 3 }
        };

        var existingTags = new List<ProductTagSQLResponse>
        {
            new() { Id = 1, ProductId = productId, TagId = 1 }
        };

        var updatedProduct = new FullProductSQLResponse
        {
            Id = productId,
            Title = updateRequest.Title,
            Description = updateRequest.Description,
            Price = (double)updateRequest.Price,
            ImageUrl = updateRequest.ImageUrl,
            UpdatedAt = now,
            CreatedAt = now
        };

        _productTagRepository.GetAllByProductIdAsync(productId, default)
            .Returns(existingTags);
        _productRepository.UpdateAsync(Arg.Any<UpdateProductSQLRequest>(), default)
            .Returns(1);
        _productRepository.GetByIdAsync(productId, default)
            .Returns(updatedProduct);

        // Act
        var result = await _sut.UpdateAsync(productId, updateRequest);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(updateRequest.Title);
        
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _productTagRepository.Received(1).DeleteAsync(1, default);
        await _productTagRepository.Received(2).CreateAsync(Arg.Any<CreateProductTagSQLRequest>(), default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }
}