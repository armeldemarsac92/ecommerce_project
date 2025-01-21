using System.Data;
using Dapper;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.SQL;

namespace Tdev702.Repository.Repository;

public interface IOrderRepository
{
    public Task<OrderSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<OrderSQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    public Task<List<OrderSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<OrderSQLResponse>> GetAllByUserIdAsync(string userId,
        CancellationToken cancellationToken = default);
    public Task<OrderSQLResponse> CreateAsync(CreateOrderSQLRequest createOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class OrderRepository : IOrderRepository
{
   private readonly IDBSQLCommand _dbCommand;

   public OrderRepository(IDBSQLCommand dbCommand)
   {
       _dbCommand = dbCommand;
   }

   public async Task<OrderSQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
   {
       return await _dbCommand.QueryFirstOrDefaultAsync<OrderSQLResponse>(OrderQueries.GetOrderById, new { OrderId = id }, cancellationToken);
   }

   public async Task<OrderSQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
   {
       return await _dbCommand.QueryFirstOrDefaultAsync<OrderSQLResponse>(OrderQueries.GetOrderByIntentId, new { StripePaymentIntentId = stripePaymentIntentId }, cancellationToken);
   }

   public async Task<List<OrderSQLResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
   {
       var response = await _dbCommand.QueryAsync<OrderSQLResponse>(OrderQueries.GetAllOrdersByUserId, new { UserId = userId },  cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSQLResponse>();
   }
   
   public async Task<List<OrderSQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
   {
       var response = await _dbCommand.QueryAsync<OrderSQLResponse>(OrderQueries.GetAllOrders, cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSQLResponse>();
   }

   public async Task<OrderSQLResponse> CreateAsync(CreateOrderSQLRequest createOrderRequest, CancellationToken cancellationToken = default)
   {
       return await _dbCommand.QuerySingleAsync<OrderSQLResponse>(OrderQueries.CreateOrder, createOrderRequest, cancellationToken);
   }

   public async Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderRequest, CancellationToken cancellationToken = default)
   {
       return await _dbCommand.ExecuteAsync(OrderQueries.UpdateOrder, updateOrderRequest, cancellationToken);
   }

   public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
   {
       await _dbCommand.ExecuteAsync(OrderQueries.DeleteOrder, new { OrderId = id }, cancellationToken);
   }
}