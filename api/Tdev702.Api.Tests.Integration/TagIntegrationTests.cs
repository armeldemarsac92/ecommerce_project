using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Tag;

namespace Tdev702.Api.Tests.Integration;

public class TagIntegrationTests : IClassFixture<TestFixture>
{
    private readonly ITagEndpoints _tagApi;
    
    public TagIntegrationTests(TestFixture fixture)
    {
        _tagApi = fixture.ServiceProvider.GetRequiredService<ITagEndpoints>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnTags()
    {
        // Act
        var response = await _tagApi.GetAllAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenTagDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentTagId = 999999;
        
        // Act
        var response = await _tagApi.GetByIdAsync(nonExistentTagId);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateTag()
    {
        // Arrange
        var createRequest = new CreateTagRequest
        {
            Title = $"Test Tag {Guid.NewGuid()}",
            Description = "Integration Test Tag"
        };
        
        // Act
        var response = await _tagApi.CreateAsync(createRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Title.Should().Be(createRequest.Title);
        response.Content.Description.Should().Be(createRequest.Description);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenTagExists_ShouldUpdateTag()
    {
        // Arrange
        // 1. Create a tag first
        var createRequest = new CreateTagRequest
        {
            Title = $"Test Tag {Guid.NewGuid()}",
            Description = "To be updated"
        };
        
        var createResponse = await _tagApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var tagId = createResponse.Content!.Id;
        
        // 2. Prepare update
        var updateRequest = new UpdateTagRequest
        {
            Title = $"Updated Tag {Guid.NewGuid()}",
            Description = "Updated description"
        };
        
        // Act
        var updateResponse = await _tagApi.UpdateAsync(tagId, updateRequest);
        
        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        updateResponse.Content!.Title.Should().Be(updateRequest.Title);
        updateResponse.Content.Description.Should().Be(updateRequest.Description);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenTagExists_ShouldDeleteTag()
    {
        // Arrange
        // 1. Create a tag first
        var createRequest = new CreateTagRequest
        {
            Title = $"Test Tag for Deletion {Guid.NewGuid()}",
            Description = "To be deleted"
        };
        
        var createResponse = await _tagApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var tagId = createResponse.Content!.Id;
        
        // Act
        var deleteResponse = await _tagApi.DeleteAsync(tagId);
        
        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify it's deleted
        var getResponse = await _tagApi.GetByIdAsync(tagId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}