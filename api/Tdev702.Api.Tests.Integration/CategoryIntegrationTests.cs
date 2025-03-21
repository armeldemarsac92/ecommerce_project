using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Category;

namespace Tdev702.Api.Tests.Integration;

public class CategoryIntegrationTests : IClassFixture<TestFixture>
{
    private readonly ICategoryEndpoints _categoryApi;
    
    public CategoryIntegrationTests(TestFixture fixture)
    {
        _categoryApi = fixture.ServiceProvider.GetRequiredService<ICategoryEndpoints>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCategories()
    {
        // Act
        var response = await _categoryApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentCategoryId = 999999;
        
        // Act
        var response = await _categoryApi.GetByIdAsync(nonExistentCategoryId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateCategory()
    {
        // Arrange
        var createRequest = new CreateCategoryRequest
        {
            Title = $"Test Category {Guid.NewGuid()}",
            Description = "Integration Test Category"
        };
        
        // Act
        var response = await _categoryApi.CreateAsync(createRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Title.Should().Be(createRequest.Title);
        response.Content.Description.Should().Be(createRequest.Description);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenCategoryExists_ShouldUpdateCategory()
    {
        // Arrange
        // 1. Create a category first
        var createRequest = new CreateCategoryRequest
        {
            Title = $"Test Category {Guid.NewGuid()}",
            Description = "To be updated"
        };
        
        var createResponse = await _categoryApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoryId = createResponse.Content!.Id;
        
        // 2. Prepare update
        var updateRequest = new UpdateCategoryRequest
        {
            Title = $"Updated Category {Guid.NewGuid()}",
            Description = "Updated description"
        };
        
        // Act
        var updateResponse = await _categoryApi.UpdateAsync(categoryId, updateRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        updateResponse.Content!.Title.Should().Be(updateRequest.Title);
        updateResponse.Content.Description.Should().Be(updateRequest.Description);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenCategoryExists_ShouldDeleteCategory()
    {
        // Arrange
        // 1. Create a category first
        var createRequest = new CreateCategoryRequest
        {
            Title = $"Test Category for Deletion {Guid.NewGuid()}",
            Description = "To be deleted"
        };
        
        var createResponse = await _categoryApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoryId = createResponse.Content!.Id;
        
        // Act
        var deleteResponse = await _categoryApi.DeleteAsync(categoryId);
        
        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify it's deleted
        var getResponse = await _categoryApi.GetByIdAsync(categoryId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}