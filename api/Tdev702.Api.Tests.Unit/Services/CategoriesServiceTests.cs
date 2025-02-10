using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Category;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

public class CategoriesServiceTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesService> _logger;
    private readonly CategoriesService _sut;

    public CategoriesServiceTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<CategoriesService>>();
        _sut = new CategoriesService(_categoryRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategoryResponse()
    {
        // Arrange
        var categoryId = 1;
        var expectedCategory = new CategorySQLResponse { Id = categoryId, Title = "Test Category" };
        var expectedResponse = new CategoryResponse { Id = categoryId, Title = "Test Category" };
        
        _categoryRepository.GetByIdAsync(categoryId, default)
            .Returns(expectedCategory);

        // Act
        var result = await _sut.GetByIdAsync(categoryId);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _categoryRepository.Received(1).GetByIdAsync(categoryId, default);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = 1;
        _categoryRepository.GetByIdAsync(categoryId, default)
            .Returns((CategorySQLResponse)null);

        // Act
        var act = () => _sut.GetByIdAsync(categoryId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {categoryId} not found");
    }

    [Fact]
    public async Task GetAllAsync_WhenCategoriesExist_ShouldReturnCategoriesList()
    {
        // Arrange
        var queryOptions = new QueryOptions();
        var sqlResponses = new List<CategorySQLResponse>
        {
            new() { Id = 1, Title = "Category 1" },
            new() { Id = 2, Title = "Category 2" }
        };
        var expectedResponses = new List<CategoryResponse>
        {
            new() { Id = 1, Title = "Category 1" },
            new() { Id = 2, Title = "Category 2" }
        };

        _categoryRepository.GetAllAsync(queryOptions, default)
            .Returns(sqlResponses);

        // Act
        var result = await _sut.GetAllAsync(queryOptions);

        // Assert
        result.Should().BeEquivalentTo(expectedResponses);
        await _categoryRepository.Received(1).GetAllAsync(queryOptions, default);
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldReturnCreatedCategory()
    {
        // Arrange
        var createRequest = new CreateCategoryRequest { Title = "New Category" };
        var createdCategoryId = 1;
        var expectedCategory = new CategorySQLResponse { Id = createdCategoryId, Title = "New Category" };
        var expectedResponse = new CategoryResponse { Id = createdCategoryId, Title = "New Category" };

        _categoryRepository.CreateAsync(Arg.Any<CreateCategorySQLRequest>(), default)
            .Returns(createdCategoryId);
        _categoryRepository.GetByIdAsync(createdCategoryId, default)
            .Returns(expectedCategory);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
        await _unitOfWork.DidNotReceive().RollbackAsync(default);
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryExists_ShouldReturnUpdatedCategory()
    {
        // Arrange
        var categoryId = 1;
        var updateRequest = new UpdateCategoryRequest { Title = "Updated Category" };
        var updatedCategory = new CategorySQLResponse { Id = categoryId, Title = "Updated Category" };
        var expectedResponse = new CategoryResponse { Id = categoryId, Title = "Updated Category" };

        _categoryRepository.UpdateAsync(Arg.Any<UpdateCategorySQLRequest>(), default)
            .Returns(1);
        _categoryRepository.GetByIdAsync(categoryId, default)
            .Returns(updatedCategory);

        // Act
        var result = await _sut.UpdateAsync(categoryId, updateRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
        await _unitOfWork.DidNotReceive().RollbackAsync(default);
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_ShouldDeleteCategory()
    {
        // Arrange
        var categoryId = 1;

        // Act
        await _sut.DeleteAsync(categoryId);

        // Assert
        await _categoryRepository.Received(1).DeleteAsync(categoryId, default);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
        await _unitOfWork.DidNotReceive().RollbackAsync(default);
    }

    [Fact]
    public async Task DeleteAsync_WhenExceptionOccurs_ShouldRollbackAndThrow()
    {
        // Arrange
        var categoryId = 1;
        _categoryRepository.DeleteAsync(categoryId, default)
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = () => _sut.DeleteAsync(categoryId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
        await _unitOfWork.DidNotReceive().CommitAsync(default);
    }
}