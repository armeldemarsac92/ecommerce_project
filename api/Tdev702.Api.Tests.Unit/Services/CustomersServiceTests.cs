using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Repository;

public class CustomerServiceTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _logger = Substitute.For<ILogger<CustomerService>>();
        _sut = new CustomerService(_customerRepository, _logger);
    }

    [Fact]
    public async Task UserExistsAsync_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        var userId = "user123";
        _customerRepository.CustomerExistsAsync(userId, default)
            .Returns(true);

        // Act
        var result = await _sut.UserExistsAsync(userId);

        // Assert
        result.Should().BeTrue();
        await _customerRepository.Received(1).CustomerExistsAsync(userId, default);
    }

    [Fact]
    public async Task UserExistsAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var userId = "user123";
        _customerRepository.CustomerExistsAsync(userId, default)
            .Returns(false);

        // Act
        var result = await _sut.UserExistsAsync(userId);

        // Assert
        result.Should().BeFalse();
        await _customerRepository.Received(1).CustomerExistsAsync(userId, default);
    }

    [Fact]
    public async Task GetAllAsync_WhenCustomersExist_ShouldReturnCustomersList()
    {
        // Arrange
        var queryOptions = new QueryOptions();
        var sqlResponses = new List<CustomerSQLResponse>
        {
            new() { Id = "user1", Email = "user1@example.com" },
            new() { Id = "user2", Email = "user2@example.com" }
        };
        var expectedResponses = new List<CustomerResponse>
        {
            new() { Id = "user1", Email = "user1@example.com" },
            new() { Id = "user2", Email = "user2@example.com" }
        };

        _customerRepository.GetAllAsync(queryOptions, default)
            .Returns(sqlResponses);

        // Act
        var result = await _sut.GetAllAsync(queryOptions);

        // Assert
        result.Should().BeEquivalentTo(expectedResponses);
        await _customerRepository.Received(1).GetAllAsync(queryOptions, default);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoCustomers_ShouldReturnEmptyList()
    {
        // Arrange
        var queryOptions = new QueryOptions();
        _customerRepository.GetAllAsync(queryOptions, default)
            .Returns(new List<CustomerSQLResponse>());

        // Act
        var result = await _sut.GetAllAsync(queryOptions);

        // Assert
        result.Should().BeEmpty();
        await _customerRepository.Received(1).GetAllAsync(queryOptions, default);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExists_ShouldReturnCustomerResponse()
    {
        // Arrange
        var userId = "user123";
        var sqlResponse = new CustomerSQLResponse 
        { 
            Id = userId, 
            Email = "user@example.com" 
        };
        var expectedResponse = new CustomerResponse 
        { 
            Id = userId, 
            Email = "user@example.com" 
        };

        _customerRepository.GetByIdAsync(userId, default)
            .Returns(sqlResponse);

        // Act
        var result = await _sut.GetByIdAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _customerRepository.Received(1).GetByIdAsync(userId, default);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = "user123";
        _customerRepository.GetByIdAsync(userId, default)
            .Returns((CustomerSQLResponse)null);

        // Act
        var act = () => _sut.GetByIdAsync(userId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Customer {userId} not found");
    }
}