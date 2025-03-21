using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;
using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Request.Product;

namespace Tdev702.Api.Tests.Integration;

public class ProductIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IProductEndpoints _productApi;
    private readonly ICategoryEndpoints _categoryApi;
    private readonly IBrandEndpoints _brandApi;

    public ProductIntegrationTests(TestFixture fixture)
    {
        _productApi = fixture.ServiceProvider.GetRequiredService<IProductEndpoints>();
        _categoryApi = fixture.ServiceProvider.GetRequiredService<ICategoryEndpoints>();
        _brandApi = fixture.ServiceProvider.GetRequiredService<IBrandEndpoints>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnProducts()
    {
        // Act
        var response = await _productApi.GetAllAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentProductId = 999999;

        // Act
        var response = await _productApi.GetByIdAsync(nonExistentProductId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldCreateProduct()
    {
        // Arrange
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "Integration Test Product",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };

        // Act
        var response = await _productApi.CreateAsync(createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Title.Should().Be(createRequest.Title);
        response.Content.Description.Should().Be(createRequest.Description);
        response.Content.Price.Should().Be(createRequest.Price);
        response.Content.ImageUrl.Should().Be(createRequest.ImageUrl);
    }

    [Fact]
    public async Task CreateAsync_WithCategoryAndBrand_ShouldCreateCompleteProduct()
    {
        // Arrange
        // 1. Create a category
        var categoryRequest = new CreateCategoryRequest
        {
            Title = $"Test Category {Guid.NewGuid()}",
            Description = "For product integration test"
        };

        var categoryResponse = await _categoryApi.CreateAsync(categoryRequest);
        categoryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoryId = categoryResponse.Content!.Id;

        // 2. Create a brand
        var brandRequest = new CreateBrandRequest
        {
            Title = $"Test Brand {Guid.NewGuid()}",
            Description = "For product integration test"
        };

        var brandResponse = await _brandApi.CreateAsync(brandRequest);
        brandResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var brandId = brandResponse.Content!.BrandId;

        // 3. Prepare product request
        var createRequest = new CreateProductRequest
        {
            Title = $"Complete Test Product {Guid.NewGuid()}",
            Description = "Product with category and brand",
            Price = 39.99,
            ImageUrl = "https://example.com/complete.jpg",
            CategoryId = categoryId,
            BrandId = brandId
        };

        // Act
        var response = await _productApi.CreateAsync(createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
        response.Content!.Title.Should().Be(createRequest.Title);
        response.Content.CategoryTitle.Should().Be(categoryRequest.Title);
        response.Content.BrandTitle.Should().Be(brandRequest.Title);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_ShouldUpdateProduct()
    {
        // Arrange
        // 1. Create a product first
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product {Guid.NewGuid()}",
            Description = "To be updated",
            Price = 29.99,
            ImageUrl = "https://example.com/test.jpg"
        };

        var createResponse = await _productApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = createResponse.Content!.Id;

        // 2. Prepare update
        var updateRequest = new UpdateProductRequest
        {
            Title = $"Updated Product {Guid.NewGuid()}",
            Description = "Updated description",
            Price = 49.99,
            ImageUrl = "https://example.com/updated.jpg"
        };

        // Act
        var updateResponse = await _productApi.UpdateAsync(productId, updateRequest);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateResponse.Content.Should().NotBeNull();
        updateResponse.Content!.Title.Should().Be(updateRequest.Title);
        updateResponse.Content.Description.Should().Be(updateRequest.Description);
        updateResponse.Content.Price.Should().Be(updateRequest.Price!.Value);
        updateResponse.Content.ImageUrl.Should().Be(updateRequest.ImageUrl);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_ShouldDeleteProduct()
    {
        // Arrange
        // 1. Create a product first
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product for Deletion {Guid.NewGuid()}",
            Description = "To be deleted",
            Price = 19.99,
            ImageUrl = "https://example.com/delete.jpg"
        };

        var createResponse = await _productApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = createResponse.Content!.Id;

        // Act
        var deleteResponse = await _productApi.DeleteAsync(productId);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify it's deleted
        var getResponse = await _productApi.GetByIdAsync(productId);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task LikeAsync_WhenProductExists_ShouldLikeProduct()
    {
        // Arrange
        // 1. Create a product first
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product for Liking {Guid.NewGuid()}",
            Description = "To be liked",
            Price = 24.99,
            ImageUrl = "https://example.com/like.jpg"
        };

        var createResponse = await _productApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = createResponse.Content!.Id;

        // Act
        await _productApi.LikeAsync(productId);

        // Get the product to verify it's liked
        var getResponse = await _productApi.GetByIdAsync(productId);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content!.IsLiked.Should().BeTrue();
    }

    [Fact]
    public async Task UnlikeAsync_WhenProductIsLiked_ShouldUnlikeProduct()
    {
        // Arrange
        // 1. Create a product first
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product for Unliking {Guid.NewGuid()}",
            Description = "To be unliked",
            Price = 14.99,
            ImageUrl = "https://example.com/unlike.jpg"
        };

        var createResponse = await _productApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = createResponse.Content!.Id;

        // 2. Like the product
        await _productApi.LikeAsync(productId);

        // 3. Verify it's liked
        var likedResponse = await _productApi.GetByIdAsync(productId);
        likedResponse.Content!.IsLiked.Should().BeTrue();

        // Act
        await _productApi.UnlikeAsync(productId);

        // Get the product to verify it's not liked anymore
        var getResponse = await _productApi.GetByIdAsync(productId);

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.Content!.IsLiked.Should().BeFalse();
    }

    [Fact]
    public async Task GetLikedProductsAsync_WhenUserHasLikedProducts_ShouldReturnLikedProducts()
    {
        // Arrange
        // 1. Create a product first
        var createRequest = new CreateProductRequest
        {
            Title = $"Test Product for Liked List {Guid.NewGuid()}",
            Description = "For liked products test",
            Price = 34.99,
            ImageUrl = "https://example.com/liked-list.jpg"
        };

        var createResponse = await _productApi.CreateAsync(createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var productId = createResponse.Content!.Id;

        // 2. Like the product
        await _productApi.LikeAsync(productId);

        // Act
        var likedProductsResponse = await _productApi.GetLikedProductsAsync();

        // Assert
        likedProductsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        likedProductsResponse.Content.Should().NotBeNull();
        likedProductsResponse.Content!.Any(p => p.Id == productId).Should().BeTrue();
    }
}