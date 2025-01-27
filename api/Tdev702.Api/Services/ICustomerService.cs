using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface ICustomerService
{
    public Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
    public Task<List<CustomerResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<CustomerResponse> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository, 
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if user exists with id: {userId}", userId);
        return await _customerRepository.CustomerExistsAsync(userId, cancellationToken);
    }

    public async Task<List<CustomerResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all customers");
        var customers = await _customerRepository.GetAllAsync(queryOptions, cancellationToken);
        return customers.MapToCustomers();
    }

    public async Task<CustomerResponse> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting customer with id: {userId}", userId);
        var customer = await _customerRepository.GetByIdAsync(userId, cancellationToken);
        if (customer is null) throw new NotFoundException($"Customer {userId} not found");
        return customer.MapToCustomer();
    }
}