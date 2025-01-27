using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IProductRepository
{
    public Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default);
    public Task<ProductSQLResponse> CreateAsync(CreateProductSQLRequest request, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateProductSQLRequest request, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class ProductRepository : IProductRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.QueryFirstOrDefaultAsync<ProductSQLResponse>(ProductQueries.GetProductById, new { Id = id }, cancellationToken);
    }

    public async Task<List<ProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryAsync<ProductSQLResponse>(ProductQueries.GetAllProducts, cancellationToken);
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }
    
    public async Task<List<ProductSQLResponse>> GetByIdsAsync(List<long> productIds, CancellationToken cancellationToken = default)
    {
        var response = await _unitOfWork.QueryAsync<ProductSQLResponse>(
            ProductQueries.GetProductsByIds, 
            new { ProductIds = productIds }, 
            cancellationToken);
    
        return response.Any() ? response.ToList() : new List<ProductSQLResponse>();
    }

    public async Task<ProductSQLResponse> CreateAsync(CreateProductSQLRequest request, CancellationToken cancellationToken = default)
    {
        var createdRowId = await _unitOfWork.QuerySingleAsync<int>(ProductQueries.CreateProduct, request, cancellationToken);
        return await _unitOfWork.QuerySingleAsync<ProductSQLResponse>(ProductQueries.GetProductById, new { Id = createdRowId }, cancellationToken);
    }

    public async Task<int> UpdateAsync(UpdateProductSQLRequest request, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteAsync(ProductQueries.UpdateProduct, request, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteAsync(ProductQueries.DeleteProduct, new { Id = id }, cancellationToken);
    }
}