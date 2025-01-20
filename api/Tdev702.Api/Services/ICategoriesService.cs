using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.Request.Shop.Category;
using Tdev702.Contracts.Response.Shop;
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

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<CategoryResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Category {id} not found");
        
        return response.MapToCategory();
    }

    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToCategories() :  throw new NotFoundException("Categories not found");
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.CreateAsync(createCategoryRequest, cancellationToken);
        return response.MapToCategory();
    }

    public async Task<CategoryResponse> UpdateAsync(long id, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    {
        updateCategoryRequest.Id = id;
        var affectedRows = await _categoryRepository.UpdateAsync(updateCategoryRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Category {id} not found");
        
        var updatedCategory = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        return updatedCategory.MapToCategory();
        
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _categoryRepository.DeleteAsync(id, cancellationToken);
    }
}