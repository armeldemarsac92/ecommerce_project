using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IStatsService
{
    public Task<long> GetNewUsersAsync(bool isDaily, CancellationToken cancellationToken = default);
    public Task<double> GetCartAverageAsync(bool isDaily, CancellationToken cancellationToken = default);
    public Task<double> GetRevenueAsync(bool isDaily,  CancellationToken cancellationToken = default);
}

public class StatsService : IStatsService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<StatsService> _logger;

    public StatsService(
        ILogger<StatsService> logger, 
        IOrderRepository orderRepository, 
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }


    public async Task<long> GetNewUsersAsync(bool isDaily, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting number of new users.");
        var (start, end) = DateRange.Get(isDaily);
        var response = await _customerRepository.GetCustomersCountAsync(start, end, cancellationToken);
        _logger.LogInformation("There are {numberOfUsers} new users.", response);
        return response;
    }

    public async Task<double> GetCartAverageAsync(bool isDaily, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting cart average.");
        var (start, end) = DateRange.Get(isDaily);
        var orders = await _orderRepository.GetAllByDateAsync(start, end, cancellationToken);
        var filteredOrders = orders.Where(order => order.StripePaymentStatus == "paid");
        var average = filteredOrders.Average(order => order.TotalAmount);
        _logger.LogInformation("The average cart value is {averageCartValue}.", average);
        return average;
    }

    public async Task<double> GetRevenueAsync(bool isDaily, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting total revenue.");
        var (start, end) = DateRange.Get(isDaily);
        var orders = await _orderRepository.GetAllByDateAsync(start, end, cancellationToken);
        var filteredOrders = orders.Where(order => order.StripePaymentStatus == "paid");
        var total = filteredOrders.Sum(order => order.TotalAmount);
        _logger.LogInformation("The total revenue is {total}.", total);
        return total;
        
    }
    
    public static class DateRange
    {
        public static (DateTime start, DateTime end) Get(bool isDaily)
        {
            var now = DateTime.UtcNow;
        
            if (isDaily)
            {
                return (now.AddHours(-24), now);
            }
        
            return (new DateTime(now.Year, now.Month, 1), now);
        }
    }
}