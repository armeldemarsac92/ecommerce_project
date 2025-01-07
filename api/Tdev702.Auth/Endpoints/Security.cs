using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tdev702.Auth.Database;
using Tdev702.Auth.Routes;

namespace Tdev702.Auth.Endpoints;

public static class SecurityEndpoints
{
    
    public static IEndpointRouteBuilder MapSecurityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Security.XsrfToken, GetXsrfToken)
            .WithName("GetXsrfToken");
        
        app.MapGet(ApiRoutes.Security.Enable, Enable2fa)
            .RequireAuthorization()
            .WithName("Enable2FA");

        app.MapPost(ApiRoutes.Security.Verify, Verify2fa)
            .RequireAuthorization()
            .WithName("Verify2FA");

        app.MapPost(ApiRoutes.Security.Disable, Disable2fa)
            .RequireAuthorization()
            .WithName("Disable2FA");
        
        return app;
    }
    
    private static IResult GetXsrfToken(IAntiforgery antiforgery, HttpContext context)
    {
        var tokens = antiforgery.GetAndStoreTokens(context);
        return Results.Ok(new { token = tokens.RequestToken });
    }
    
    private static async Task<IResult> Enable2fa(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.NotFound();

        // Generate the authenticator key
        var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        }

        var authenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);

        return Results.Ok(new
        {
            SharedKey = unformattedKey,
            AuthenticatorUri = authenticatorUri
        });
    }

    private static async Task<IResult> Verify2fa(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ClaimsPrincipal claimsPrincipal,
        [FromBody] string code)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.NotFound();

        var verificationCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
            user, TokenOptions.DefaultAuthenticatorProvider, verificationCode);

        if (!is2faTokenValid)
            return Results.BadRequest("Verification code is invalid.");

        await userManager.SetTwoFactorEnabledAsync(user, true);

        return Results.Ok("2FA has been enabled.");
    }

    private static async Task<IResult> Disable2fa(
        UserManager<User> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.NotFound();

        var disable2faResult = await userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2faResult.Succeeded)
            return Results.BadRequest(disable2faResult.Errors);

        return Results.Ok("2FA has been disabled.");
    }

    private static string GenerateQrCodeUri(string email, string unformattedKey)
    {
        const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        
        return string.Format(
            AuthenticatorUriFormat,
            Uri.EscapeDataString("Your App Name"),
            Uri.EscapeDataString(email),
            unformattedKey);
    }

}