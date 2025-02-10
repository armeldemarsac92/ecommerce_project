using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tdev702.Api.Services;
using Tdev702.Contracts.API.Request.Tag;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Tag;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

public class TagsServiceTests
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TagsService> _logger;
    private readonly TagsService _sut;

    public TagsServiceTests()
    {
        _tagRepository = Substitute.For<ITagRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<TagsService>>();
        _sut = new TagsService(_tagRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTagExists_ShouldReturnTag()
    {
        // Arrange
        var tagId = 1L;
        var tagResponse = new TagSQLResponse 
        { 
            Id = tagId,
            Title = "Test Tag"
        };

        _tagRepository.GetByIdAsync(tagId, default)
            .Returns(tagResponse);

        // Act
        var result = await _sut.GetByIdAsync(tagId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(tagId);
        result.Title.Should().Be("Test Tag");
    }

    [Fact]
    public async Task GetByIdAsync_WhenTagDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var tagId = 1L;
        _tagRepository.GetByIdAsync(tagId, default)
            .Returns((TagSQLResponse)null);

        // Act
        var act = () => _sut.GetByIdAsync(tagId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product Tags {tagId} not found");
    }

    [Fact]
    public async Task GetAllAsync_WhenTagsExist_ShouldReturnTags()
    {
        // Arrange
        var queryOptions = new QueryOptions();
        var tags = new List<TagSQLResponse>
        {
            new() { Id = 1, Title = "Tag 1" },
            new() { Id = 2, Title = "Tag 2" }
        };

        _tagRepository.GetAllAsync(queryOptions, default)
            .Returns(tags);

        // Act
        var result = await _sut.GetAllAsync(queryOptions);

        // Assert
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[1].Id.Should().Be(2);
    }

    [Fact]
    public async Task CreateAsync_WhenSuccessful_ShouldReturnCreatedTag()
    {
        // Arrange
        var createRequest = new CreateTagRequest { Title = "New Tag" };
        var createdTagId = 1;
        var tagResponse = new TagSQLResponse 
        { 
            Id = createdTagId,
            Title = "New Tag"
        };

        _tagRepository.CreateAsync(Arg.Any<CreateTagSQLRequest>(), default)
            .Returns(createdTagId);
        _tagRepository.GetByIdAsync(createdTagId, default)
            .Returns(tagResponse);

        // Act
        var result = await _sut.CreateAsync(createRequest);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(createdTagId);
        result.Title.Should().Be(createRequest.Title);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task CreateAsync_WhenExceptionOccurs_ShouldRollbackAndThrow()
    {
        // Arrange
        var createRequest = new CreateTagRequest { Title = "New Tag" };
        _tagRepository.CreateAsync(Arg.Any<CreateTagSQLRequest>(), default)
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = () => _sut.CreateAsync(createRequest);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
        await _unitOfWork.DidNotReceive().CommitAsync(default);
    }

    [Fact]
    public async Task UpdateAsync_WhenTagExists_ShouldReturnUpdatedTag()
    {
        // Arrange
        var tagId = 1;
        var updateRequest = new UpdateTagRequest { Title = "Updated Tag" };
        var updatedTag = new TagSQLResponse 
        { 
            Id = tagId,
            Title = "Updated Tag"
        };

        _tagRepository.UpdateAsync(Arg.Any<UpdateTagSQLRequest>(), default)
            .Returns(1);
        _tagRepository.GetByIdAsync(tagId, default)
            .Returns(updatedTag);

        // Act
        var result = await _sut.UpdateAsync(tagId, updateRequest);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(tagId);
        result.Title.Should().Be(updateRequest.Title);
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task UpdateAsync_WhenTagDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var tagId = 1;
        var updateRequest = new UpdateTagRequest { Title = "Updated Tag" };
        _tagRepository.UpdateAsync(Arg.Any<UpdateTagSQLRequest>(), default)
            .Returns(0);

        // Act
        var act = () => _sut.UpdateAsync(tagId, updateRequest);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product Tag {tagId} not found");
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
    }

    [Fact]
    public async Task DeleteAsync_WhenTagExists_ShouldDeleteSuccessfully()
    {
        // Arrange
        var tagId = 1;
        _tagRepository.DeleteAsync(tagId, default)
            .Returns(1);

        // Act
        await _sut.DeleteAsync(tagId);

        // Assert
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).CommitAsync(default);
    }

    [Fact]
    public async Task DeleteAsync_WhenTagDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var tagId = 1L;
        _tagRepository.DeleteAsync(tagId, default)
            .Returns(0);

        // Act
        var act = () => _sut.DeleteAsync(tagId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product Tag {tagId} not found");
        await _unitOfWork.Received(1).BeginTransactionAsync(default);
        await _unitOfWork.Received(1).RollbackAsync(default);
    }
}