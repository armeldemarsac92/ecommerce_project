using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IOrderService
{
    Task<OrderResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default);
    Task<long> GetOrderIdByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    
    Task<List<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<List<OrderResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<OrderResponse> CreateAsync(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken = default);
    
    Task<OrderResponse> UpdateAsync(UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken = default);
}
public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;


    public OrderService(
        ILogger<OrderService> logger, 
        IOrderRepository orderRepository,
        IProductRepository productRepository, 
        IOrderProductRepository orderProductRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
    }
    
    public async Task<OrderResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order {orderId} not found");
        var orderProducts = await _orderProductRepository.GetAllByOrderId(orderId, cancellationToken);
        var productIds = orderProducts.Select(op => op.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
        var mappedOrderProducts = orderProducts.MapToOrderProducts(products);
        var mappedOrder = order.MapToOrder(mappedOrderProducts);
        return mappedOrder; //to refactor, too much code duplication
    }

    public async Task<long> GetOrderIdByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByPaymentIntentIdAsync(stripePaymentIntentId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order with payment intent {stripePaymentIntentId} not found");
        return order.Id;
    }

    public async Task<List<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var orderProducts = await _orderProductRepository.GetAllAsync(cancellationToken);
        var productIds = orderProducts.Select(op => op.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
        var mappedOrderProducts = orderProducts.MapToOrderProducts(products);
        var mappedOrders = orders.MapToOrders(mappedOrderProducts);
        return mappedOrders;
    }
    
    public async Task<List<OrderResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var orderProducts = await _orderProductRepository.GetAllAsync(cancellationToken);
        var productIds = orderProducts.Select(op => op.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
        var mappedOrderProducts = orderProducts.MapToOrderProducts(products);
        var mappedOrders = orders.MapToOrders(mappedOrderProducts);
        return mappedOrders;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest createOrderRequest,
        CancellationToken cancellationToken = default)
    {
        var products =
            await _productRepository.GetByIdsAsync(createOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        var totalAmount = products.Sum(p =>
            p.Price * createOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);
        var createOrderSqlRequest = createOrderRequest.MapToCreateOrderRequest(totalAmount);
        var createOrderSqlResponse = await _orderRepository.CreateAsync(createOrderSqlRequest, cancellationToken);
        var createOrderProductsSqlRequest =
            createOrderRequest.Products.MapToCreateOrderProductRequests(products, createOrderSqlResponse.Id);
        var orderProductResponse =
            await _orderProductRepository.CreateManyAsync(createOrderProductsSqlRequest, cancellationToken);
        var mappedOrderProducts = orderProductResponse.MapToOrderProducts(products);
        var order = createOrderSqlResponse.MapToOrder(mappedOrderProducts);
        return order;
    }

    public async Task<OrderResponse> UpdateAsync(UpdateOrderRequest updateOrderRequest,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(updateOrderRequest.Id, cancellationToken);
        if (order is null) throw new NotFoundException($"Order {updateOrderRequest.Id} not found");
        if (order.PaymentStatus is "succeeded") throw new BadRequestException("Cannot update an order with a succeeded payment status");
        var products =
            await _productRepository.GetByIdsAsync(updateOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        var updateOrderProductsSqlRequest =
            updateOrderRequest.Products.MapToUpdateOrderProductRequests(products, updateOrderRequest.Id);
        await _orderProductRepository.UpdateManyAsync(updateOrderProductsSqlRequest,
            cancellationToken);

        var totalAmount = products.Sum(p =>
            p.Price * updateOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);
        var updateOrderSqlRequest = updateOrderRequest.MapToUpdateOrderRequest(updateOrderRequest.Id, totalAmount);
        var affectedOrderRows = await _orderRepository.UpdateAsync(updateOrderSqlRequest, cancellationToken);
        if (affectedOrderRows == 0) throw new InvalidOperationException("Update failed");
        return await GetByIdAsync(updateOrderRequest.Id, cancellationToken);
    }
}