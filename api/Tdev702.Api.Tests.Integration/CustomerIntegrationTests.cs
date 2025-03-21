using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;

namespace Tdev702.Api.Tests.Integration.Customers;

public class CustomerIntegrationTests : IClassFixture<TestFixture>
{
    private readonly ICustomerEndpoints _customerApi;
    
    public CustomerIntegrationTests(TestFixture fixture)
    {
        _customerApi = fixture.ServiceProvider.GetRequiredService<ICustomerEndpoints>();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnCustomers()
    {
        // Act
        var response = await _customerApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllAsync_WithPagination_ShouldReturnPaginatedCustomers()
    {
        // Arrange
        string pageSize = "5";
        string pageNumber = "1";
        
        // Act
        var response = await _customerApi.GetAllAsync(pageSize, pageNumber);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Count.Should().BeLessThanOrEqualTo(5); // Should respect page size
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenCustomerDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentCustomerId = "non-existent-id";
        
        // Act
        var response = await _customerApi.GetByIdAsync(nonExistentCustomerId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenCustomerExists_ShouldReturnCustomer()
    {
        // Note: This test assumes there's a way to retrieve a valid customer ID.
        // In a real integration test, you might need to:
        // - Access a test user with known ID
        // - Use a known test user ID from test configuration
        // - Query all customers and use the first one
        
        // For this example, we'll first get all customers and then use an ID from the result
        // if available.
        
        // Arrange
        var allCustomersResponse = await _customerApi.GetAllAsync();
        
        // Skip the test if no customers are available
        if (allCustomersResponse.Content == null || !allCustomersResponse.Content.Any())
        {
            // This is a conditional skip, you might use Skip attribute or other mechanisms
            // in a real test scenario
            return;
        }
        
        var firstCustomerId = allCustomersResponse.Content!.First().Id;
        
        // Act
        var response = await _customerApi.GetByIdAsync(firstCustomerId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Id.Should().Be(firstCustomerId);
    }
}