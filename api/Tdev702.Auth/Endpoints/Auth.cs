using System.Security.Claims;
using MassTransit;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Tdev702.Auth.Extensions;
using Tdev702.Auth.Routes;
using Tdev702.Auth.Services;
using Tdev702.AWS.SDK.SES;
using Tdev702.Contracts.Auth;
using Tdev702.Contracts.Auth.Request;
using Tdev702.Contracts.Config;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Auth.Endpoints;

public static class AuthEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Auth";
    
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Auth.Login, Login)
            .Accepts<LoginRequest>(ContentType)
            .Produces<AccessTokenResponse>()
            .WithName("Login")
            .WithTags(Tags);        
        
        app.MapPost(ApiRoutes.Auth.Register, Register)
            .Accepts<RegisterUserRequest>(ContentType)
            .WithName("Register")
            .WithTags(Tags);
        
        app.MapPost(ApiRoutes.Auth.Update, Update)
            .Accepts<UpdateUserRequest>(ContentType)
            .RequireAuthorization("Authenticated")
            .WithName("Update")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.Refresh, RefreshToken)
            .Accepts<RefreshTokenRequest>(ContentType)
            .Produces<AccessTokenResponse>()
            .WithName("RefreshToken")
            .RequireAuthorization("Authenticated")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.ResendConfirmation, Send2FaCode)
            .Accepts<Get2FaCodeRequest>(ContentType)
            .WithName("ResendConfirmation")
            .WithTags(Tags);
        
        app.MapGet(ApiRoutes.Auth.ExternalLogin, OAuthLogin)
            .WithName("ExternalLogin")
            .WithTags(Tags);
        
        app.MapGet(ApiRoutes.Auth.ExternalCallback, OAuthCallback)
            .WithName("ExternalCallback")
            .Produces<AccessTokenResponse>()
            .WithTags(Tags);
        
        app.MapGet(ApiRoutes.Auth.HealthCheck, () => Results.Ok("Healthy"))
           .WithName("HealthCheck")
           .WithTags(Tags);

        return app;
    }

    private static async Task<IResult> Login(
       IUserService userService,
       LoginUserRequest request)
    {
        var token = await userService.LoginUserAsync(request);
        return Results.Ok(token);
    }
        
    private static async Task<IResult> Register(
        IUserService userService,
        RegisterUserRequest request)
    {
        await userService.RegisterUserAsync(request.Email, "User");
        return Results.Ok(new { requiresTwoFactor = true, provider = "Email" });
    }    
    
    private static async Task<IResult> Update(
        IUserService userService,
        ClaimsPrincipal claims,
        UpdateUserRequest request)
    {
        return Results.Ok(await userService.UpdateUserAsync(claims, request));
    }

    private static async Task<IResult> RefreshToken(
        UserManager<User> userManager,
        ITokenService tokenService,
        RefreshTokenRequest request)
    {
        return Results.Ok(await tokenService.RefreshTokenAsync(request.RefreshToken));
    }

    private static async Task<IResult> Send2FaCode(
        IUserService userService,
        Get2FaCodeRequest request)
    {
        await userService.Send2FaCodeAsync(request);
        return Results.Ok(new { requiresTwoFactor = true, provider = "Email" });
    }
    
    private static async Task<IResult> OAuthLogin(
        string provider,
        AuthConfiguration configuration,
        IAuthService authService,
        ISecurityService securityService)
    {
        var authParameters = new AuthenticationParameters(provider);

        await securityService.StoreAuthState(authParameters);
        var loginUri = authService.BuildLoginUri(authParameters);
        return Results.Ok(loginUri);
    }

    private static async Task<IResult> OAuthCallback(
    HttpContext context,
    IConfiguration config,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ISecurityService securityService,
    IUserService userService,
    IAuthService authService,
    ITokenService tokenService,
    IHttpClientFactory httpClientFactory)
    {
        
        var state = context.GetUriParameterFromHttpContext("state");
        var authStateData = await securityService.ValidateState(state);
        var authParameters = authStateData.AuthenticationParameters;
        
        var code = context.GetUriParameterFromHttpContext("code");
        authParameters.AuthorizationCode = code;
        var tokenResponse = await authService.ExchangeCodeForTokens(authParameters);
        authParameters.AccessToken = tokenResponse.AccessToken;

        var userInfos = await authService.GetUserInfosAsync(authParameters);
        
        var userRole = "User";
        if(authParameters.IdentityProvider == "aws") userRole = "Admin";
        
        var user = await userManager.FindByEmailAsync(userInfos.Email);
        if (user != null)
        {
            await userManager.UpdateAsync(new User()
            {
                Email = userInfos.Email, FirstName = userInfos.GivenName, LastName = userInfos.FamilyName,
                ProfilePicture = userInfos.Picture
            });

            await userManager.AddToRoleAsync(user, userRole);
            
            var accessTokenResponse = await tokenService.GetAccessTokenAsync(user);
            
            AddCookies(context, accessTokenResponse);

            return Results.Redirect(authParameters.FrontEndRedirectUri);

        }
        
        var newUser = await userService.ProvisionUserAsync(new UserRecord(userInfos.GivenName, userInfos.FamilyName, userInfos.Email, true, userInfos.Picture, ""), userRole);

        var info = new UserLoginInfo(authParameters.IdentityProvider, userInfos.Sub, authParameters.IdentityProvider);
        var addLoginResult = await userManager.AddLoginAsync(newUser, info);
        if (!addLoginResult.Succeeded)
        {
            throw new Exception("Failed to add external login");
        }

        var accessTokenResponse2 = await tokenService.GetAccessTokenAsync(newUser);
            
        AddCookies(context, accessTokenResponse2);
        
        return Results.Redirect(authParameters.FrontEndRedirectUri);
      }

    private static void AddCookies(HttpContext context, AccessTokenResponse accessTokenResponse)
    {
        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        
        context.Response.Cookies.Append("access_token", accessTokenResponse.AccessToken, new CookieOptions
        {
            HttpOnly = false,  
            Secure = true,
            SameSite = SameSiteMode.Lax,   
            Expires = DateTimeOffset.UtcNow.AddHours(2),
            Domain = isDev ? ".localhost":".epitechproject.fr",
            Path = "/"
        });

        context.Response.Cookies.Append("refresh_token", accessTokenResponse.RefreshToken, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Domain = isDev ? ".localhost":".epitechproject.fr",
            Path = "/"
        });
    }
}