using Microsoft.AspNetCore.Antiforgery;
using Tdev702.Auth.Routes;

namespace Tdev702.Auth.Endpoints;

public static class SecurityEndpoints
{
    
    public static IEndpointRouteBuilder MapSecurityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Security.XsrfToken, GetXsrfToken)
            .WithName("GetXsrfToken");
        
        return app;
    }
    
    private static IResult GetXsrfToken(IAntiforgery antiforgery, HttpContext context)
    {
        var tokens = antiforgery.GetAndStoreTokens(context);
        return Results.Ok(new { token = tokens.RequestToken });
    }

}