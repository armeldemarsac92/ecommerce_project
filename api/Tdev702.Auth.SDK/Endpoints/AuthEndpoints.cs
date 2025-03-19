using Microsoft.AspNetCore.Authentication.BearerToken;
using Refit;
using Tdev702.Auth.Routes;
using Tdev702.Contracts.Auth.Request;

namespace Tdev702.Auth.SDK.Endpoints;

public interface IAuthEndpoints
{
    [Post(ApiRoutes.Auth.Login)]
    Task<ApiResponse<AccessTokenResponse>> LoginAsync([Body] LoginUserRequest request, CancellationToken cancellationToken = default);
    
    [Post(ApiRoutes.Auth.Refresh)]
    Task<ApiResponse<AccessTokenResponse>> RefreshTokenAsync([Body] RefreshTokenRequest request, CancellationToken cancellationToken = default);
    
    [Post(ApiRoutes.Auth.Send2FaCode)] 
    Task Send2FaCodeAsync([Body] Get2FaCodeRequest request, CancellationToken cancellationToken = default);
}