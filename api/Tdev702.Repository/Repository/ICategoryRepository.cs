using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Category;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ICategoryRepository
{
    public Task<CategorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<CategorySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateCategorySQLRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateCategorySQLRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbContext _dbContext;

    public CategoryRepository(IDbContext unitOfWork)
    {
        _dbContext = unitOfWork;
    }
    
    public async Task<CategorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<CategorySQLResponse>(CategoryQueries.GetCategoryById, new { Id = id }, cancellationToken);
    }

    public async Task<List<CategorySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<CategorySQLResponse>(CategoryQueries.GetAllCategories, queryOptions, cancellationToken);
        return response.Any() ? response.ToList() : new List<CategorySQLResponse>();
    }

    public async Task<int> CreateAsync(CreateCategorySQLRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(CategoryQueries.CreateCategory, createCategoryRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateCategorySQLRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbContext.ExecuteAsync(CategoryQueries.UpdateCategory, updateCategoryRequest, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _dbContext.ExecuteAsync(CategoryQueries.DeleteCategory, new { Id = id }, cancellationToken);
    }
}