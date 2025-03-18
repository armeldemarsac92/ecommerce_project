using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tdev702.Api.SDK.Endpoints;
using Tdev702.Api.Tests.Integration.Fixture;

namespace Tdev702.Api.Tests.Integration.Products;

public class ProductIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IProductEndpoints _productsApi;
    public ProductIntegrationTests( 
        TestFixture fixture)
    {
        _productsApi = fixture.ServiceProvider.GetRequiredService<IProductEndpoints>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
    {
        // Arrange
        var productId = 1;

        // Act
        var productResponse = await _productsApi.GetByIdAsync(productId);
        var product = productResponse.Content;

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().Be(productId);
    }

}