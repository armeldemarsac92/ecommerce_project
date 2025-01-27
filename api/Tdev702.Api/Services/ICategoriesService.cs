using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ICategoriesService
{
    public Task<CategoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<CategoryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<CategoryResponse> UpdateAsync(long categoryId, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<CategoriesService> logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<CategoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting category with id: {id}", id);
        var response = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Category {id} not found");
        
        return response.MapToCategory();
    }

    public async Task<List<CategoryResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all categories");
        var response = await _categoryRepository.GetAllAsync(queryOptions, cancellationToken);
        return response.Any() ? response.MapToCategories() :  throw new NotFoundException("Categories not found");
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new category {categoryName}", createCategoryRequest.Title);
        var sqlRequest = createCategoryRequest.MapToCreateCategoryRequest();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var createdCategoryId = await _categoryRepository.CreateAsync(sqlRequest, cancellationToken);
            var categoryResponse = await _categoryRepository.GetByIdAsync(createdCategoryId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Category {categoryId} created successfully.", createdCategoryId);
            return categoryResponse.MapToCategory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating category {categoryName}: {message}", createCategoryRequest.Title, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<CategoryResponse> UpdateAsync(long categoryId, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating category with id: {categoryId}", categoryId);
        var sqlRequest = updateCategoryRequest.MapToUpdateCategoryRequest(categoryId);
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _categoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Category {categoryId} not found");
            var updatedCategory = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Category {categoryId} updated successfully.", categoryId);
            return updatedCategory.MapToCategory();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating category {categoryId}: {message}", categoryId, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
       
        
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting category with id: {categoryId}", id);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _categoryRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Category {categoryId} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting category {categoryId}: {message}", id, ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}