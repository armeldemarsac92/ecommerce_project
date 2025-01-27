using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Brand;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IBrandRepository
{
    public Task<BrandSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<BrandSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateBrandSQLRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateBrandSQLRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class BrandRepository : IBrandRepository
{
    private readonly IDbContext _dbContext;
    public BrandRepository(IUnitOfWork unitOfWork, IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BrandSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<BrandSQLResponse>(BrandQueries.GetBrandById, new { BrandId = id }, cancellationToken);
    }

    public async Task<List<BrandSQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var response = await _dbContext.QueryAsync<BrandSQLResponse>(BrandQueries.GetAllBrands, queryOptions, cancellationToken);
        return response.Any() ? response.ToList() : new List<BrandSQLResponse>();
    }

    public async Task<int> CreateAsync(CreateBrandSQLRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(BrandQueries.CreateBrand, createBrandRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateBrandSQLRequest updateBrandRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbContext.ExecuteAsync(BrandQueries.UpdateBrand, updateBrandRequest, cancellationToken);
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
         await _dbContext.ExecuteAsync(BrandQueries.DeleteBrand, new { BrandId = brandId}, cancellationToken);
    }
}