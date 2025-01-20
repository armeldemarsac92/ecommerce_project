using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.Brand;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Brands;

public interface IBrandRepository
{
    public Task<BrandSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<BrandSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<BrandSQLResponse> CreateAsync(CreateBrandSQLRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateBrandSQLRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class BrandRepository : IBrandRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public BrandRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<BrandSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<BrandSQLResponse>(BrandQueries.GetBrandById, new { BrandId = id }, cancellationToken);
    }

    public async Task<List<BrandSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<BrandSQLResponse>(BrandQueries.GetAllBrands, cancellationToken);
        return response.Any() ? response.ToList() : new List<BrandSQLResponse>();
    }

    public async Task<BrandSQLResponse> CreateAsync(CreateBrandSQLRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<BrandSQLResponse>(BrandQueries.CreateBrand, createBrandRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateBrandSQLRequest updateBrandRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbCommand.ExecuteAsync(BrandQueries.UpdateBrand, updateBrandRequest, cancellationToken);
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
         await _dbCommand.ExecuteAsync(BrandQueries.DeleteBrand, new { BrandId = brandId}, cancellationToken);
    }
}