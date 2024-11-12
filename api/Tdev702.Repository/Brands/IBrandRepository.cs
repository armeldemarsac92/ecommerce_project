using Tdev702.Contracts.SQL.Request.Shop;
using Tdev702.Contracts.SQL.Request.Shop.Brand;
using Tdev702.Contracts.SQL.Response.Shop;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Brands;

public interface IBrandRepository
{
    public Task<BrandResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<BrandResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class BrandRepository : IBrandRepository
{
    private readonly IDBSQLCommand _dbCommand;

    public BrandRepository(IDBSQLCommand dbCommand)
    {
        _dbCommand = dbCommand;
    }

    public async Task<BrandResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QueryFirstOrDefaultAsync<BrandResponse>(BrandQueries.GetBrandById, new { BrandId = id }, cancellationToken);
    }

    public async Task<List<BrandResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _dbCommand.QueryAsync<BrandResponse>(BrandQueries.GetAllBrands, cancellationToken);
        return response.Any() ? response.ToList() : new List<BrandResponse>();
    }

    public async Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        return await _dbCommand.QuerySingleAsync<BrandResponse>(BrandQueries.CreateBrand, createBrandRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default)
    { 
        return await _dbCommand.ExecuteAsync(BrandQueries.UpdateBrand, updateBrandRequest, cancellationToken);
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
         await _dbCommand.ExecuteAsync(BrandQueries.DeleteBrand, new { BrandId = brandId}, cancellationToken);
    }
}