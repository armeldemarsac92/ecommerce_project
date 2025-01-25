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
    public Task<OrderSummarySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<OrderSummarySQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    public Task<List<OrderSummarySQLResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<List<OrderSummarySQLResponse>> GetAllByUserIdAsync(string userId,
        CancellationToken cancellationToken = default);
    public Task<OrderSummarySQLResponse> CreateAsync(CreateOrderSQLRequest createOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderSqlRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class OrderRepository : IOrderRepository
{
   private readonly IUnitOfWork _unitOfWork;

   public OrderRepository(IUnitOfWork unitOfWork)
   {
       _unitOfWork = unitOfWork;
   }

   public async Task<OrderSummarySQLResponse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
   {
       return await _unitOfWork.QueryFirstOrDefaultAsync<OrderSummarySQLResponse>(OrderQueries.GetOrderById, new { OrderId = id }, cancellationToken);
   }

   public async Task<OrderSummarySQLResponse?> GetByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
   {
       return await _unitOfWork.QueryFirstOrDefaultAsync<OrderSummarySQLResponse>(OrderQueries.GetOrderByIntentId, new { StripePaymentIntentId = stripePaymentIntentId }, cancellationToken);
   }

   public async Task<List<OrderSummarySQLResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
   {
       var response = await _unitOfWork.QueryAsync<OrderSummarySQLResponse>(OrderQueries.GetAllOrdersByUserId, new { UserId = userId },  cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSummarySQLResponse>();
   }
   
   public async Task<List<OrderSummarySQLResponse>> GetAllAsync(CancellationToken cancellationToken = default)
   {
       var response = await _unitOfWork.QueryAsync<OrderSummarySQLResponse>(OrderQueries.GetAllOrders, cancellationToken);
       return response.Any() ? response.ToList() : new List<OrderSummarySQLResponse>();
   }

   public async Task<OrderSummarySQLResponse> CreateAsync(CreateOrderSQLRequest createOrderRequest, CancellationToken cancellationToken = default)
   {
       var createdRowId = await _unitOfWork.QuerySingleAsync<int>(OrderQueries.CreateOrder, createOrderRequest, cancellationToken);
       return await _unitOfWork.QuerySingleAsync<OrderSummarySQLResponse>(OrderQueries.GetOrderById, new { OrderId = createdRowId }, cancellationToken);
   }

   public async Task<int> UpdateAsync(UpdateOrderSQLRequest updateOrderRequest, CancellationToken cancellationToken = default)
   {
       return await _unitOfWork.ExecuteAsync(OrderQueries.UpdateOrder, updateOrderRequest, cancellationToken);
   }

   public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
   {
       await _unitOfWork.ExecuteAsync(OrderQueries.DeleteOrder, new { OrderId = id }, cancellationToken);
   }
}