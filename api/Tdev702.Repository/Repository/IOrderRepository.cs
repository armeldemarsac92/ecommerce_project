using System.Data;
using Dapper;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Context;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IOrderRepository
{
    public Task<OrderSummarySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<OrderSummarySQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    public Task<List<OrderSummarySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<List<OrderSummarySQLResponse>> GetAllByUserIdAsync(string userId, QueryOptions queryOptions,
        CancellationToken cancellationToken = default);
    public Task<int> CreateAsync(CreateOrderSQLRequest createOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class OrderRepository : IOrderRepository
{
   private readonly IDbContext _dbContext;

   public OrderRepository(IDbContext dbContext)
   {
       _dbContext = dbContext;
   }

   public async Task<OrderSummarySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
   {
       return await _dbContext.QueryFirstOrDefaultAsync<OrderSummarySQLResponse>(OrderQueries.GetOrderById, new { OrderId = id }, cancellationToken);
   }

   public async Task<OrderSummarySQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
   {
       return await _dbContext.QueryFirstOrDefaultAsync<OrderSummarySQLResponse>(OrderQueries.GetOrderByIntentId, new { StripePaymentIntentId = stripePaymentIntentId }, cancellationToken);
   }

   public async Task<List<OrderSummarySQLResponse>> GetAllByUserIdAsync(string userId, QueryOptions queryOptions, CancellationToken cancellationToken = default)
   {
       var response = await _dbContext.QueryAsync<OrderSummarySQLResponse>(OrderQueries.GetAllOrdersByUserId, new { UserId = userId, PageSisze = queryOptions.PageSize, OffSet = queryOptions.Offset, OrderBy = queryOptions.SortBy },  cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSummarySQLResponse>();
   }
   
   public async Task<List<OrderSummarySQLResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
   {
       var response = await _dbContext.QueryAsync<OrderSummarySQLResponse>(OrderQueries.GetAllOrders, queryOptions, cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSummarySQLResponse>();
   }

   public async Task<int> CreateAsync(CreateOrderSQLRequest createOrderRequest, CancellationToken cancellationToken = default)
   {
       return await _dbContext.QuerySingleAsync<int>(OrderQueries.CreateOrder, createOrderRequest, cancellationToken);
   }

   public async Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderRequest, CancellationToken cancellationToken = default)
   {
       return await _dbContext.ExecuteAsync(OrderQueries.UpdateOrder, updateOrderRequest, cancellationToken);
   }

   public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
   {
       await _dbContext.ExecuteAsync(OrderQueries.DeleteOrder, new { OrderId = id }, cancellationToken);
   }
}