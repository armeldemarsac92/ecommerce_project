using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Brand;

namespace Tdev702.Api.Tests.Integration;

public class BrandIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IBrandEndpoints _brandApi;
    
    public BrandIntegrationTests(TestFixture fixture)
    {
        _brandApi = fixture.ServiceProvider.GetRequiredService<IBrandEndpoints>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnBrands()
    {
        // Act
        var response = await _brandApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenBrandDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentBrandId = 999999;
        
        // Act
        var response = await _brandApi.GetByIdAsync(nonExistentBrandId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateBrand()
    {
        // Arrange
        var createRequest = new CreateBrandRequest
        {
            Title = $"Test Brand {Guid.NewGuid()}",
            Description = "Integration Test Brand"
        };
        
        // Act
        var response = await _brandApi.CreateAsync(createRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Title.Should().Be(createRequest.Title);
        response.Content.Description.Should().Be(createRequest.Description);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenBrandExists_ShouldUpdateBrand()
    {
        // Arrange
        // 1. Create a brand first
        var createRequest = new CreateBrandRequest
        {
            Title = $"Test Brand {Guid.NewGuid()}",
            Description = "To be updated"
        };
        
        var createResponse = await _brandApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var brandId = createResponse.Content!.BrandId;
        
        // 2. Prepare update
        var updateRequest = new UpdateBrandRequest
        {
            Title = $"Updated Brand {Guid.NewGuid()}",
            Description = "Updated description"
        };
        
        // Act
        var updateResponse = await _brandApi.UpdateAsync(brandId, updateRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        updateResponse.Content!.Title.Should().Be(updateRequest.Title);
        updateResponse.Content.Description.Should().Be(updateRequest.Description);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenBrandExists_ShouldDeleteBrand()
    {
        // Arrange
        // 1. Create a brand first
        var createRequest = new CreateBrandRequest
        {
            Title = $"Test Brand for Deletion {Guid.NewGuid()}",
            Description = "To be deleted"
        };
        
        var createResponse = await _brandApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var brandId = createResponse.Content!.BrandId;
        
        // Act
        var deleteResponse = await _brandApi.DeleteAsync(brandId);
        
        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify it's deleted
        var getResponse = await _brandApi.GetByIdAsync(brandId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}