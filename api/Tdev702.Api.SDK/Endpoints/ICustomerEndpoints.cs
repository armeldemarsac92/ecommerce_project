using Refit;
using Tdev702.Api.Routes;
using Tdev702.Contracts.API.Response;

namespace Tdev702.Api.SDK.Endpoints;

public interface ICustomerEndpoints
{
    [Get(ShopRoutes.Customers.GetAll)]
    Task<ApiResponse<List<CustomerResponse>>> GetAllAsync(string? pageSize = null, string? pageNumber = null, string? sortBy = null, CancellationToken cancellationToken = default);
    
    [Get(ShopRoutes.Customers.GetById)]
    Task<ApiResponse<CustomerResponse>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}