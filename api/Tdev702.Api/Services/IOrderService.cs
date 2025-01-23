using Stripe;
using Tdev702.Contracts.API.Request.Order;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.Order;
using Tdev702.Contracts.SQL.Response;
using Tdev702.Repository.Repository;
using Tdev702.Stripe.SDK.Services;

namespace Tdev702.Api.Services;

public interface IOrderService
{
    Task<OrderSummaryResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default);
    Task<OrderSummaryResponse> GetOrderByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default);
    
    Task<List<OrderSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<List<OrderSummaryResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<OrderSummaryResponse> CreateAsync(CreateOrderRequest createOrderRequest, CancellationToken cancellationToken = default);

    Task UpdateOrderPaymentStatus(UpdateOrderSQLRequest updateOrderRequest,
        CancellationToken cancellationToken = default);
    
    Task<OrderSummaryResponse> UpdateAsync(long orderId, UpdateOrderRequest updateOrderRequest, CancellationToken cancellationToken = default);
}
public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IStripePaymentIntentService _stripePaymentIntentService;


    public OrderService(
        ILogger<OrderService> logger, 
        IOrderRepository orderRepository,
        IProductRepository productRepository, 
        IOrderProductRepository orderProductRepository, 
        IStripePaymentIntentService stripePaymentIntentService)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _stripePaymentIntentService = stripePaymentIntentService;
    }
    
    public async Task<OrderSummaryResponse> GetByIdAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order {orderId} not found");
        return order.MapToOrderSummary();
    }

    public async Task<OrderSummaryResponse> GetOrderByPaymentIntentIdAsync(string stripePaymentIntentId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByPaymentIntentIdAsync(stripePaymentIntentId, cancellationToken);
        if(order is null) throw new NotFoundException($"Order with payment intent {stripePaymentIntentId} not found");
        return order.MapToOrderSummary();
    }



    public async Task<List<OrderSummaryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.MapToOrderSummaries();
    }



    public async Task<List<OrderSummaryResponse>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllByUserIdAsync(userId, cancellationToken);
        return orders.MapToOrderSummaries();
    }

    public async Task<OrderSummaryResponse> CreateAsync(CreateOrderRequest createOrderRequest,
        CancellationToken cancellationToken = default)
    {
        //first we retrieve the products linked to the order to be created
        var products =
            await _productRepository.GetByIdsAsync(createOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        
        //we calculate the total amount based on the products prices and quantities, not written in the request
        var totalAmount = products.Sum(p =>
            p.Price * createOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);
        
        //we prepare the order data for insertion into the SQL database
        var createOrderSqlRequest = createOrderRequest.MapToCreateOrderRequest(totalAmount);
        
        //we create the order in the SQL database
        var createOrderSqlResponse = await _orderRepository.CreateAsync(createOrderSqlRequest, cancellationToken);
        
        //we then use the id of the created order to create the corresponding links between the order and the products
        var createOrderProductsSqlRequest =
            createOrderRequest.Products.MapToCreateOrderProductRequests(products, createOrderSqlResponse.Id);
        
        //we create the corresponding links between the order and the products in the SQL database
        await _orderProductRepository.CreateManyAsync(createOrderProductsSqlRequest, cancellationToken);
        
        //we map the created order products to their corresponding products and the order
        var order = createOrderSqlResponse.MapToOrderSummary();
        return order;
    }
    
    public async Task UpdateOrderPaymentStatus(UpdateOrderSQLRequest updateOrderRequest, CancellationToken cancellationToken)
    {
        await _orderRepository.UpdateAsync(updateOrderRequest, cancellationToken);
    }

    public async Task<OrderSummaryResponse> UpdateAsync(long orderId, UpdateOrderRequest updateOrderRequest,
        CancellationToken cancellationToken = default)
    {
        //first we retrieve the order
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null) throw new NotFoundException($"Order {orderId} not found");
        if (order.PaymentStatus is "succeeded") throw new BadRequestException("Cannot update an order with a succeeded payment status");
        
        //then we retrieve the products linked to the order request to ensure the prices are up to date
        var newProducts =
            await _productRepository.GetByIdsAsync(updateOrderRequest.Products.Select(p => p.ProductId).ToList(),
                cancellationToken);
        
        //we calculate the new total amount based on the updated products prices and quantities
        var totalAmount = newProducts.Sum(p =>
            p.Price * updateOrderRequest.Products.First(op => op.ProductId == p.Id).Quantity);

        //we update the order payment intent with the new total amount
        await UpdateOrderPaymentIntent(cancellationToken, totalAmount, order);

        //we remove any products that have been removed from the order request from the SQL database
        await RemoveDeatchedProducts(cancellationToken, order, newProducts);

        //we update the link between the order and the products with the new prices and quantities
        var updateOrderProductsSqlRequest =
            updateOrderRequest.Products.MapToUpdateOrderProductRequests(newProducts, orderId);
        await _orderProductRepository.UpdateManyAsync(updateOrderProductsSqlRequest,
            cancellationToken);
        
        //then we update the order itself with the new total amount that we calculated earlier
        var updateOrderSqlRequest = updateOrderRequest.MapToUpdateOrderRequest(orderId, totalAmount);
        var affectedOrderRows = await _orderRepository.UpdateAsync(updateOrderSqlRequest, cancellationToken);
        if (affectedOrderRows == 0) throw new InvalidOperationException("Update failed");
        var updatedRow = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        return updatedRow.MapToOrderSummary();
    }

    private async Task RemoveDeatchedProducts(CancellationToken cancellationToken, OrderSummarySQLResponse order, List<ProductSQLResponse> newProducts)
    {
        //then we retrieve the old products linked to the order to see if some products have been removed
        var oldProductOrderList = await _orderProductRepository.GetAllByOrderId(order.Id, cancellationToken);
        var oldProducts =
            await _productRepository.GetByIdsAsync(oldProductOrderList.Select(op => op.ProductId).ToList(),
                cancellationToken);
        
        var productsToRemove = oldProducts.Where(op => newProducts.All(np => np.Id != op.Id)).ToList();
        var productsLinksToRemove = oldProductOrderList.Where(op => productsToRemove.Any(np => np.Id == op.ProductId)).ToList();

        foreach (var productLink in productsLinksToRemove)
        {
            await _orderProductRepository.DeleteAsync(productLink.Id, cancellationToken);
        }
    }

    private async Task UpdateOrderPaymentIntent(CancellationToken cancellationToken, double totalAmount,
        OrderSummarySQLResponse order)
    {
        //if the new total amount is different from the current one, we update the payment intent amount
        if (Math.Abs(totalAmount - order.TotalAmount) > 1 && !string.IsNullOrEmpty(order.StripePaymentIntentId))
        {
            await _stripePaymentIntentService.UpdateAsync(
                order.StripePaymentIntentId, 
                new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(totalAmount * 100), //stripe requires amounts in cents
                    Description = $"Updated order {order.Id}" 
                }, 
                null, 
                cancellationToken);
        }
    }
}