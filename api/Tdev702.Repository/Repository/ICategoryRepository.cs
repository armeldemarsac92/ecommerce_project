using Tdev702.Contracts.SQL.Request.Shop.Category;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ICategoryRepository
{
    public Task<CategoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public CategoryRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }
    
    public async Task<CategoryResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<CategoryResponse>(CategoryQueries.GetCategoryById, new { CategoryId = id }, cancellationToken);
    }

    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<CategoryResponse>(CategoryQueries.GetAllCategories, cancellationToken);
        return response.Any() ? response.ToList() : new List<CategoryResponse>();
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<CategoryResponse>(CategoryQueries.CreateCategory, createCategoryRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbCommand.ExecuteAsync(CategoryQueries.UpdateCategory, updateCategoryRequest, cancellationToken);
    }

    public async Task DeleteAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        await _dbCommand.ExecuteAsync(CategoryQueries.DeleteCategory, categoryId, cancellationToken);
    }
}