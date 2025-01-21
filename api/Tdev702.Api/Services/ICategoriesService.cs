using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ICategoriesService
{
    public Task<CategoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<CategoryResponse> UpdateAsync(long id, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(ICategoryRepository categoryRepository, ILogger<CategoriesService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }
    
    public async Task<CategoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {   
            _logger.LogInformation("Trying to fetch  Category: {id}", id);
            var response = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if(response is null) throw new NotFoundException($"Category {id} not found");
            _logger.LogInformation("Successfully fetched  Category: {id}", id);
            return response.MapToCategory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching Category by id: {Message}", e.Message);
            throw;
        }
    }

    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {   
            _logger.LogInformation("Trying to fetch all Categories");
            var response = await _categoryRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched all Categories");
            return response.Any() ? response.MapToCategories() :  throw new NotFoundException("Categories not found");
        }
        catch (Exception e)
        {
            _logger.LogError("Error fetching all Categories: {Message}", e.Message);
            throw;
        }
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to create Category: {Name}", createCategoryRequest.Title);
            var sqlRequest = createCategoryRequest.MapToCreateCategoryRequest();
            var response = await _categoryRepository.CreateAsync(sqlRequest, cancellationToken);
            _logger.LogInformation("Successfully created Category: {Name}", createCategoryRequest.Title);
            return response.MapToCategory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating Category: {Message}", e.Message);
            throw;
        }
        
    }

    public async Task<CategoryResponse> UpdateAsync(long id, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to modify Category: {Id}", id);
            var sqlRequest = updateCategoryRequest.MapToUpdateCategoryRequest(id);
            var affectedRows = await _categoryRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Category {id} not found");
            var updatedCategory = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            _logger.LogInformation("Successfully updated Category: {Id}", id);
            return updatedCategory.MapToCategory();
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating Category: {Message}", e.Message);
            throw;
        }
        
        
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to delete Category: {Id}", id);
            await _categoryRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted Category: {Id}", id);
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting Category: {Message}", e.Message);
            throw;
        }
        
    }
}