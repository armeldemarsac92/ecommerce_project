using Tdev702.Contracts.API.Request.Category;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.Category;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface ICategoryRepository
{
    public Task<CategorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<CategorySQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<CategorySQLResponse> CreateAsync(CreateCategorySQLRequest createCategoryRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateCategorySQLRequest updateCategoryRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CategorySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.QueryFirstOrDefaultAsync<CategorySQLResponse>(CategoryQueries.GetCategoryById, new { Id = id }, cancellationToken);
    }

    public async Task<List<CategorySQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryAsync<CategorySQLResponse>(CategoryQueries.GetAllCategories, cancellationToken);
        return response.Any() ? response.ToList() : new List<CategorySQLResponse>();
    }

    public async Task<CategorySQLResponse> CreateAsync(CreateCategorySQLRequest createCategoryRequest, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.QuerySingleAsync<CategorySQLResponse>(CategoryQueries.CreateCategory, createCategoryRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateCategorySQLRequest updateCategoryRequest, CancellationToken cancellationToken = default)
    { 
        return await _unitOfWork.ExecuteAsync(CategoryQueries.UpdateCategory, updateCategoryRequest, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteAsync(CategoryQueries.DeleteCategory, new { Id = id }, cancellationToken);
    }
}