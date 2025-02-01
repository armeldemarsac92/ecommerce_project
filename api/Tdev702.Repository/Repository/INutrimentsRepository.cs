using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface INutrimentsRepository
{
    public Task<NutrimentSQLResponse?> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateNutrimentSQLRequest createNutrimentRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateNutrimentSQLRequest updateNutrimentRequest, CancellationToken cancellationToken = default);
}

public class NutrimentsRepository : INutrimentsRepository
{
    private readonly IDbContext _dbContext;

    public NutrimentsRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NutrimentSQLResponse?> GetByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<NutrimentSQLResponse>(NutrimentQueries.GetByProductId,
            new { Id = productId }, cancellationToken);
    }

    public async Task<int> CreateAsync(CreateNutrimentSQLRequest createNutrimentRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.QuerySingleAsync<int>(NutrimentQueries.Create,
            createNutrimentRequest, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateNutrimentSQLRequest updateNutrimentRequest, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ExecuteAsync(NutrimentQueries.Update, updateNutrimentRequest,
            cancellationToken);
    }
}