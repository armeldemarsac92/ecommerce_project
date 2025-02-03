using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.OrderProduct;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IOrderProductRepository
{
    public Task<OrderProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<OrderProductSQLResponse>> GetAllByOrderIdAsync(long orderId, CancellationToken cancellationToken = default);
    public Task<List<OrderProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<OrderProductSQLResponse> CreateAsync(CreateOrderProductSQLRequest createOrderProductSqlRequest, CancellationToken cancellationToken = default);
    public Task<List<OrderProductSQLResponse>> CreateManyAsync(List<CreateOrderProductSQLRequest> orderProducts,
        CancellationToken cancellationToken = default);    
    
    public Task<int> UpdateManyAsync(List<UpdateOrderProductSQLRequest> orderProducts,
        CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateOrderProductSQLRequest updateOrderProductSqlRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class OrderProductRepository : IOrderProductRepository
{
   private readonly IDbContext _dbContext;

   public OrderProductRepository(IDbContext dbContext)
   {
       _dbContext = dbContext;
   }

   public async Task<OrderProductSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
   {
       return await _dbContext.QueryFirstOrDefaultAsync<OrderProductSQLResponse>(OrderProductQueries.GetOrderProductById, new { OrderProductId = id }, cancellationToken);
   }

   public async Task<List<OrderProductSQLResponse>> GetAllByOrderIdAsync(long orderId, CancellationToken cancellationToken = default)
   {
       var response = await _dbContext.QueryAsync<OrderProductSQLResponse>(OrderProductQueries.GetAllOrderProductsByOrderId, orderId, cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderProductSQLResponse>();
   }

   public async Task<List<OrderProductSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
   {
       var response = await _dbContext.QueryAsync<OrderProductSQLResponse>(OrderProductQueries.GetAllOrderProducts, cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderProductSQLResponse>();
   }

   public async Task<OrderProductSQLResponse> CreateAsync(CreateOrderProductSQLRequest createOrderProductRequest, CancellationToken cancellationToken = default)
   {
       return await _dbContext.QuerySingleAsync<OrderProductSQLResponse>(OrderProductQueries.CreateOrderProduct, createOrderProductRequest, cancellationToken);
   }
   
   public async Task<List<OrderProductSQLResponse>> CreateManyAsync(List<CreateOrderProductSQLRequest> orderProducts, CancellationToken cancellationToken = default)
   {
       var parameters = new
       {
           ProductIds = orderProducts.Select(op => op.ProductId).ToArray(),
           OrderIds = orderProducts.Select(op => op.OrderId).ToArray(),
           Quantities = orderProducts.Select(op => op.Quantity).ToArray(),
           UnitPrices = orderProducts.Select(op => op.UnitPrice).ToArray(),
           Subtotals = orderProducts.Select(op => op.Subtotal).ToArray()
       };

       var response = await _dbContext.QueryAsync<OrderProductSQLResponse>(
           OrderProductQueries.CreateManyOrderProducts,
           parameters,
           cancellationToken);

       return response.ToList();
   }

   public async Task<int> UpdateManyAsync(List<UpdateOrderProductSQLRequest> orderProducts, CancellationToken cancellationToken = default)
   {
       var parameters = new
       {
           ProductIds = orderProducts.Select(op => op.ProductId).ToArray(),
           OrderIds = orderProducts.Select(op => op.OrderId).ToArray(),
           Quantities = orderProducts.Select(op => op.Quantity).ToArray(),
           Subtotals = orderProducts.Select(op => op.Subtotal).ToArray()
       };
       
       return await _dbContext.ExecuteAsync(
           OrderProductQueries.UpdateManyOrderProducts,
           parameters,
           cancellationToken);

   }

   public async Task<int> UpdateAsync(UpdateOrderProductSQLRequest updateOrderProductRequest, CancellationToken cancellationToken = default)
   {
       return await _dbContext.ExecuteAsync(OrderProductQueries.UpdateOrderProduct, updateOrderProductRequest, cancellationToken);
   }

   public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
   {
       await _dbContext.ExecuteAsync(OrderProductQueries.DeleteOrderProduct, new { OrderProductId = id }, cancellationToken);
   }
}