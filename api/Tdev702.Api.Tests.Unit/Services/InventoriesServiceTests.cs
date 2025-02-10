using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Inventory;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Inventory;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

public class InventoriesServiceTests
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InventoriesService> _logger;
    private readonly InventoriesService _sut;

    public InventoriesServiceTests()
    {
        _inventoryRepository = Substitute.For<IInventoryRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<InventoriesService>>();
        _sut = new InventoriesService(_inventoryRepository, _productRepository, _logger, _unitOfWork);
    }

    [Fact]
    public async Task GetByIdAsync_WhenInventoryExists_ShouldReturnInventoryResponse()
    {
        // Arrange
        var inventoryId = 1;
        var productId = 1;
        var sqlResponse = new InventorySQLResponse { Id = inventoryId, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };
        var expectedResponse = new InventoryResponse { Id = inventoryId, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };

        _inventoryRepository.GetByIdAsync(inventoryId, default)
            .Returns(sqlResponse);

        // Act
        var result = await _sut.GetByIdAsync(inventoryId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetByProductIdAsync_WhenInventoryExists_ShouldReturnInventoryResponse()
    {
        // Arrange
        var productId = 1;
        var sqlResponse = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };
        var expectedResponse = new InventoryResponse { Id = 1, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };

        _inventoryRepository.GetInventoryByProductIdAsync(productId, default)
            .Returns(sqlResponse);

        // Act
        var result = await _sut.GetByProductIdAsync(productId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldReturnCreatedInventory()
    {
        // Arrange
        var createRequest = new CreateInventoryRequest { ProductId = 1, Quantity = 10 };
        var createdId = 1;
        var sqlResponse = new InventorySQLResponse { Id = createdId, ProductId = 1, Quantity = 10, Sku = new Guid().ToString() };
        var expectedResponse = new InventoryResponse { Id = createdId, ProductId = 1, Quantity = 10, Sku = new Guid().ToString() };

        _inventoryRepository.CreateAsync(Arg.Any<CreateInventorySQLRequest>(), default)
            .Returns(createdId);
        _inventoryRepository.GetByIdAsync(createdId, default)
            .Returns(sqlResponse);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task IncreamentAsync_WhenSuccessful_ShouldReturnUpdatedInventory()
    {
        // Arrange
        var productId = 1;
        var addQuantity = 5;
        var currentInventory = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };
        var updatedInventory = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 15, Sku = new Guid().ToString() };

        _inventoryRepository.GetInventoryByProductIdAsync(productId, default)
            .Returns(currentInventory);
        _inventoryRepository.UpdateAsync(Arg.Any<UpdateInventorySQLRequest>(), default)
            .Returns(1);
        _inventoryRepository.GetByIdAsync(1, default)
            .Returns(updatedInventory);

        // Act
        var result = await _sut.IncreamentAsync(addQuantity, productId);

        // Assert
        result.Quantity.Should().Be(15);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task DecrementAsync_WhenQuantityIsValid_ShouldReturnUpdatedInventory()
    {
        // Arrange
        var productId = 1;
        var subtractQuantity = 5;
        var currentInventory = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 10, Sku = new Guid().ToString() };
        var updatedInventory = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 5, Sku = new Guid().ToString() };

        _inventoryRepository.GetInventoryByProductIdAsync(productId, default)
            .Returns(currentInventory);
        _inventoryRepository.UpdateAsync(Arg.Any<UpdateInventorySQLRequest>(), default)
            .Returns(1);
        _inventoryRepository.GetByIdAsync(1, default)
            .Returns(updatedInventory);

        // Act
        var result = await _sut.DecrementAsync(subtractQuantity, productId);

        // Assert
        result.Quantity.Should().Be(5);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task DecrementAsync_WhenSubtractedQuantityExceedsAvailable_ShouldThrowBadRequestException()
    {
        // Arrange
        var productId = 1;
        var subtractQuantity = 15;
        var currentInventory = new InventorySQLResponse { Id = 1, ProductId = productId, Quantity = 10, Sku = new Guid().ToString()};

        _inventoryRepository.GetInventoryByProductIdAsync(productId, default)
            .Returns(currentInventory);

        // Act
        var act = () => _sut.DecrementAsync(subtractQuantity, productId);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage($"Substracted Quantity ({subtractQuantity}) is superior to the actual quantity ({currentInventory.Quantity})");
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_ShouldDeleteInventory()
    {
        // Arrange
        var inventoryId = 1;

        // Act
        await _sut.DeleteAsync(inventoryId);

        // Assert
        await _inventoryRepository.Received(1).DeleteAsync(inventoryId, default);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task DeleteAsync_WhenExceptionOccurs_ShouldRollback()
    {
        // Arrange
        var inventoryId = 1;
        _inventoryRepository.DeleteAsync(inventoryId, default)
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = () => _sut.DeleteAsync(inventoryId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
        await _unitOfWork.DidNotReceive().CommitAsync(default);
    }
}