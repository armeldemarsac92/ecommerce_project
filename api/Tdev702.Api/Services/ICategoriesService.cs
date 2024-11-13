using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Category;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ICategoriesService
{
    public Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Category> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<Category> UpdateAsync(long id, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if(response is null) throw new NotFoundException($"Category {id} not found");
        
        return response.MapToCategory();
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToCategories() :  throw new NotFoundException("Categories not found");
    }

    public async Task<Category> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        var response = await _categoryRepository.CreateAsync(createCategoryRequest, cancellationToken);
        return response.MapToCategory();
    }

    public async Task<Category> UpdateAsync(long id, UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default)
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