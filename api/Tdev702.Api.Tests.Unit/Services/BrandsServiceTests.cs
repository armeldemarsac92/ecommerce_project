using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.Brand;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

public class BrandsServiceTests
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BrandsService> _logger;
    private readonly BrandsService _sut;

    public BrandsServiceTests()
    {
        _brandRepository = Substitute.For<IBrandRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<BrandsService>>();
        _sut = new BrandsService(_brandRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBrandExists_ShouldReturnBrandResponse()
    {
        // Arrange
        var brandId = 1;
        var expectedBrand = new BrandSQLResponse { BrandId = brandId, Title = "Test Brand" };
        var expectedResponse = new BrandResponse { BrandId = brandId, Title = "Test Brand" };
        
        _brandRepository.GetByIdAsync(brandId, default)
            .Returns(expectedBrand);

        // Act
        var result = await _sut.GetByIdAsync(brandId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _brandRepository.Received(1).GetByIdAsync(brandId, default);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBrandDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var brandId = 1;
        _brandRepository.GetByIdAsync(brandId, default)
            .Returns((BrandSQLResponse)null);

        // Act
        var act = () => _sut.GetByIdAsync(brandId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Brand {brandId} not found");
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldReturnCreatedBrand()
    {
        // Arrange
        var createRequest = new CreateBrandRequest { Title = "New Brand" };
        var createdBrandId = 1;
        var expectedBrand = new BrandSQLResponse { BrandId = createdBrandId, Title = "New Brand" };
        var expectedResponse = new BrandResponse { BrandId = createdBrandId, Title = "New Brand" };

        _brandRepository.CreateAsync(Arg.Any<CreateBrandSQLRequest>(), default)
            .Returns(createdBrandId);
        _brandRepository.GetByIdAsync(createdBrandId, default)
            .Returns(expectedBrand);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
        await _unitOfWork.DidNotReceive().RollbackAsync(default);
    }

    [Fact]
    public async Task CreateAsync_WhenExceptionOccurs_ShouldRollbackAndThrow()
    {
        // Arrange
        var createRequest = new CreateBrandRequest { Title = "New Brand" };
        _brandRepository.CreateAsync(Arg.Any<CreateBrandSQLRequest>(), default)
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = () => _sut.CreateAsync(createRequest);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
        await _unitOfWork.DidNotReceive().CommitAsync(default);
    }
}