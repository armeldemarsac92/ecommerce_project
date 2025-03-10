using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Refit;
using System.Net;
using Tdev702.Api.Services;
using Tdev702.Contracts.OpenFoodFact.Request;
using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.OpenFoodFact.SDK.Endpoints;

public class OpenFoodFactServiceTests
{
    private readonly IProductSearchEndpoints _productSearchEndpoints;
    private readonly ILogger<OpenFoodFactService> _logger;
    private readonly OpenFoodFactService _sut;

    public OpenFoodFactServiceTests()
    {
        _productSearchEndpoints = Substitute.For<IProductSearchEndpoints>();
        _logger = Substitute.For<ILogger<OpenFoodFactService>>();
        _sut = new OpenFoodFactService(_productSearchEndpoints, _logger);
    }

    [Fact]
    public async Task SearchProductAsync_WhenSuccessful_ShouldReturnSearchResult()
    {
        // Arrange
        var searchParams = new ProductSearchParams { Category = "chocolate" };
        var expectedContent = new OpenFoodFactSearchResult 
        { 
            Count = 1,
            Page = 1,
            PageSize = 10,
            PageCount = 1,
            Products = new List<OpenFoodFactPartialProduct> 
            { 
                new() 
                { 
                    Code = "123456",
                    ImageUrl = "http://example.com/image.jpg",
                    NutritionGrades = "a",
                    ProductNameFr = "Chocolat Noir" 
                } 
            }
        };

        var apiResponse = new ApiResponse<OpenFoodFactSearchResult>(
            new HttpResponseMessage(HttpStatusCode.OK),
            expectedContent,
            new RefitSettings());

        _productSearchEndpoints.SearchProducts(searchParams, default)
            .Returns(apiResponse);

        // Act
        var result = await _sut.SearchProductAsync(searchParams);

        // Assert
        result.Should().BeEquivalentTo(expectedContent);
        await _productSearchEndpoints.Received(1).SearchProducts(searchParams, default);
    }

    [Fact]
    public async Task SearchProductAsync_WhenRequestFails_ShouldThrowHttpRequestException()
    {
        // Arrange
        var searchParams = new ProductSearchParams { Category = "chocolate" };
        var apiResponse = new ApiResponse<OpenFoodFactSearchResult>(
            new HttpResponseMessage(HttpStatusCode.BadRequest),
            null,
            new RefitSettings());

        _productSearchEndpoints.SearchProducts(searchParams, default)
            .Returns(apiResponse);

        // Act
        var act = () => _sut.SearchProductAsync(searchParams);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage($"Failed to search for product with query options: {searchParams}");
    }

    [Fact]
    public async Task GetProductByBarCodeAsync_WhenSuccessful_ShouldReturnProduct()
    {
        // Arrange
        var barcode = "1234567890";
        var expectedProduct = new OpenFoodFactProduct 
        { 
            Code = barcode,
            ImageUrl = "http://example.com/image.jpg",
            NutritionGrades = "a",
            ProductNameFr = "Chocolat Noir"
        };
        var productResponse = new OpenFoodFactProductResult 
        { 
            Code = barcode,
            Product = expectedProduct 
        };

        var apiResponse = new ApiResponse<OpenFoodFactProductResult>(
            new HttpResponseMessage(HttpStatusCode.OK),
            productResponse,
            new RefitSettings());

        _productSearchEndpoints.GetProductByBarCode(barcode, default)
            .Returns(apiResponse);

        // Act
        var result = await _sut.GetProductByBarCodeAsync(barcode);

        // Assert
        result.Should().BeEquivalentTo(expectedProduct);
        await _productSearchEndpoints.Received(1).GetProductByBarCode(barcode, default);
    }

    [Fact]
    public async Task GetProductByBarCodeAsync_WhenRequestFails_ShouldThrowHttpRequestException()
    {
        // Arrange
        var barcode = "1234567890";
        var apiResponse = new ApiResponse<OpenFoodFactProductResult>(
            new HttpResponseMessage(HttpStatusCode.NotFound),
            null,
            new RefitSettings());

        _productSearchEndpoints.GetProductByBarCode(barcode, default)
            .Returns(apiResponse);

        // Act
        var act = () => _sut.GetProductByBarCodeAsync(barcode);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage($"Failed to get product by barcode: {barcode}");
    }
}