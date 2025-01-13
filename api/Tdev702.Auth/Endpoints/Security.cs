using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tdev702.Auth.Database;
using Tdev702.Auth.Models;
using Tdev702.Auth.Routes;
using Tdev702.AWS.SDK.SES;

namespace Tdev702.Auth.Endpoints;

public static class SecurityEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Security";
    public static IEndpointRouteBuilder MapSecurityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Security.XsrfToken, GetXsrfToken)
            .WithName("GetXsrfToken")
            .WithTags(Tags);
        
        app.MapPost(ApiRoutes.Security.Enable, Setup2FA)
            // .Accepts<Setup2FaRequest>(ContentType)
            .RequireAuthorization()
            .WithName("Enable2FA")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Security.Verify, Verify2fa)
            // .Accepts<string>(ContentType)
            .RequireAuthorization()
            .WithName("Verify2FA")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Security.Disable, Disable2fa)
            .RequireAuthorization()
            .WithName("Disable2FA")
            .WithTags(Tags);
        
        return app;
    }
    
    private static IResult GetXsrfToken(IAntiforgery antiforgery, HttpContext context)
    {
        var tokens = antiforgery.GetAndStoreTokens(context);
        return Results.Ok(new { token = tokens.RequestToken });
    }
    
    private static async Task<IResult> Setup2FA(
        UserManager<User> userManager,
        ClaimsPrincipal claimsPrincipal,
        Setup2FaRequest request,
        IEmailService emailService)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
            return Results.NotFound();

        switch (request.Type)
        {
            case TwoFactorType.Authenticator:
                return await SetupAuthenticator(userManager, user);
                
            case TwoFactorType.Email:
                return await SetupEmail(userManager, user, emailService);
                
            case TwoFactorType.SMS:
                if (string.IsNullOrEmpty(request.PhoneNumber))
                    return Results.BadRequest("Phone number is required for SMS 2FA");
                return await SetupSMS(userManager, user, request.PhoneNumber);
                
            default:
                return Results.BadRequest("Invalid 2FA type");
        }
    }

    private static async Task<IResult> SetupAuthenticator(UserManager<User> userManager, User user)
    {
        var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
        }

        var authenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);

        return Results.Ok(new
        {
            Type = TwoFactorType.Authenticator,
            SharedKey = unformattedKey,
            AuthenticatorUri = authenticatorUri
        });
    }

    private static async Task<IResult> SetupEmail(UserManager<User> userManager, User user, IEmailService emailService)
    {
        var token = await userManager.GenerateTwoFactorTokenAsync(user, 
            TokenOptions.DefaultEmailProvider);
        
        await emailService.SendVerificationCodeAsync(user.Email!, token);
        
        return Results.Ok(new
        {
            Type = TwoFactorType.Email,
            Message = "Verification code sent to your email"
        });
    }

    private static async Task<IResult> SetupSMS(UserManager<User> userManager, User user, string phoneNumber)
    {
        var setPhoneResult = await userManager.SetPhoneNumberAsync(user, phoneNumber);
        if (!setPhoneResult.Succeeded)
            return Results.BadRequest(setPhoneResult.Errors);

        var token = await userManager.GenerateTwoFactorTokenAsync(user, 
            TokenOptions.DefaultPhoneProvider);
        
        // TODO: Send SMS with token
        
        return Results.Ok(new
        {
            Type = TwoFactorType.SMS,
            Message = "Verification code sent to your phone"
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
            Uri.EscapeDataString("Epitech Project"),
            Uri.EscapeDataString(email),
            unformattedKey);
    }

}