using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Tdev702.Auth.Database;
using Tdev702.Auth.Models;
using Tdev702.Auth.Routes;
using Tdev702.Auth.Services;
using Tdev702.AWS.SDK.SES;
using Tdev702.Contracts.SQL.Request.Auth;
using CreateUserRequest = Tdev702.Contracts.SQL.Request.Auth.CreateUserRequest;

namespace Tdev702.Auth.Endpoints;

public static class AuthEndpoints
{
    private const string ContentType = "application/json";
    private const string Tags = "Auth";
    
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Auth.Login, Login)
            .Accepts<LoginRequest>(ContentType)
            .WithName("Login")
            .WithTags(Tags);
        
        app.MapPost(ApiRoutes.Auth.Verify2FA, Verify2Fa)
            .Accepts<Verify2FaRequest>(ContentType)
            .WithName("VerifyCode2FA")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.Register, Register)
            .Accepts<RegisterRequest>(ContentType)
            .WithName("Register")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.Refresh, RefreshToken)
            .Accepts<RefreshTokenRequest>(ContentType)
            .WithName("RefreshToken")
            .WithTags(Tags);

        app.MapGet(ApiRoutes.Auth.ConfirmEmail, ConfirmEmail)
            .WithName("ConfirmEmail")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.ResendConfirmation, ResendConfirmation)
            .Accepts<ResendConfirmationRequest>(ContentType)
            .WithName("ResendConfirmation")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.ForgotPassword, ForgotPassword)
            .Accepts<ForgotPasswordRequest>(ContentType)
            .WithName("ForgotPassword")
            .WithTags(Tags);

        app.MapPost(ApiRoutes.Auth.ResetPassword, ResetPassword)
            .Accepts<ResetPasswordRequest>(ContentType)
            .WithName("ResetPassword")
            .WithTags(Tags);

        return app;
    }

    private static async Task<IResult> Login(
       UserManager<User> userManager,
       ITokenService tokenService,
       ClaimsPrincipal claimsPrincipal,
       SignInManager<User> signInManager,
       IEmailService emailService,
       LoginUserRequest request)
    {
       var result = await signInManager.PasswordSignInAsync(
           request.Email,
           request.Password,
           isPersistent: false,
           lockoutOnFailure: true);

       if (result.IsLockedOut)
       {
           return Results.BadRequest("Account is locked. Please try again later.");
       }

       if (result.RequiresTwoFactor)
       {
           var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
           if (user == null)
           {
               return Results.BadRequest("Invalid credentials");
           }

           switch (user.PreferredTwoFactorProvider)
           {
               case TwoFactorType.Email:
                   var emailToken = await signInManager.UserManager.GenerateTwoFactorTokenAsync(user, "Email");
                   await emailService.SendEmailAsync(
                       user.Email,
                       "2FA Code",
                       $"Your verification code is: {emailToken}");
                   return Results.Ok(new { requiresTwoFactor = true, provider = "Email" });

               case TwoFactorType.Authenticator:
                   if (!string.IsNullOrEmpty(request.TwoFactorCode))
                   {
                       var isValid = await userManager.VerifyTwoFactorTokenAsync(user, 
                           TokenOptions.DefaultAuthenticatorProvider, 
                           request.TwoFactorCode);

                       if (isValid)
                       {
                           var accessToken = tokenService.GenerateAccessToken(user);
                           var refreshToken = tokenService.GenerateRefreshToken(user);
                           return Results.Ok(new { token = accessToken, refreshToken = refreshToken });
                       }
                       return Results.BadRequest("Invalid authenticator code");
                   }
                   return Results.Ok(new { requiresTwoFactor = true, provider = "Authenticator" });

               case TwoFactorType.SMS:
                   var phoneToken = await signInManager.UserManager.GenerateTwoFactorTokenAsync(user, "Phone");
                   // Assuming you have an SMS sender service
                   // await smsSender.SendSmsAsync(user.PhoneNumber, $"Your verification code is: {phoneToken}");
                   return Results.Ok(new { requiresTwoFactor = true, provider = "Phone" });

               default:
                   return Results.BadRequest("Invalid 2FA provider");
           }
       }

       if (result.Succeeded)
       {
           var user = await userManager.FindByEmailAsync(request.Email);
           var accessToken = tokenService.GenerateAccessToken(user);
           var refreshToken = tokenService.GenerateRefreshToken(user);

           return Results.Ok(new 
           { 
               token = accessToken,
               refreshToken = refreshToken
           });
       }

       return Results.BadRequest("Invalid credentials");
    }
    
    private static async Task<IResult> Verify2Fa(
        UserManager<User> userManager,
        ITokenService tokenService,
        SignInManager<User> signInManager,
        VerifyFaRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Results.BadRequest("Invalid request");
        }

        bool isValid = false;
        switch (user.PreferredTwoFactorProvider)
        {
            case TwoFactorType.Email:
                isValid = await userManager.VerifyTwoFactorTokenAsync(user, "Email", request.VerificationCode);
                break;
            
            case TwoFactorType.SMS:
                isValid = await userManager.VerifyTwoFactorTokenAsync(user, "Phone", request.VerificationCode);
                break;
        }

        if (!isValid)
        {
            return Results.BadRequest("Invalid verification code");
        }

        // Generate tokens after successful 2FA
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(user);

        return Results.Ok(new 
        { 
            token = accessToken,
            refreshToken = refreshToken
        });
    }
        
    private static async Task<IResult> Register(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IEmailSender<User> emailSender,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        RegisterUserRequest request)
    {
        var user = new User { UserName = request.Email, Email = request.Email, FirstName = request.FirstName, LastName = request.LastName };
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);
        
        await userManager.AddToRoleAsync(user, "User");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var confirmationLink = linkGenerator.GetUriByName(
            httpContext,
            "ConfirmEmail",
            new { userId = user.Id, token = encodedToken });

        await emailSender.SendConfirmationLinkAsync(
            user,
            user.Email,
            $"Please confirm your email by clicking this link: {confirmationLink}");

        return Results.Ok("Registration successful. Please check your email for confirmation.");
    }

    private static async Task<IResult> RefreshToken(
        UserManager<User> userManager,
        ITokenService tokenService,
        RefreshTokenRequest request)
    {
        try
        {
            var principal = tokenService.ValidateToken(request.RefreshToken, validateLifetime: false);
        
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Results.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Results.Unauthorized();

            return Results.Ok(new
            {
                token = tokenService.GenerateAccessToken(user),
                refreshToken = tokenService.GenerateRefreshToken(user)
            });
        }
        catch (Exception)
        {
            return Results.Unauthorized();
        }
    }

    private static async Task<IResult> ConfirmEmail(
        UserManager<User> userManager,
        string userId,
        string token)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return Results.NotFound();

        var decodedToken = Uri.UnescapeDataString(token);
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded) return Results.BadRequest(result.Errors);

        return Results.Ok("Email confirmed successfully");
    }

    private static async Task<IResult> ResendConfirmation(
        UserManager<User> userManager,
        IEmailSender<User> emailSender,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        ResendConfirmationRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Results.NotFound();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var confirmationLink = linkGenerator.GetUriByName(
            httpContext,
            "ConfirmEmail",
            new { userId = user.Id, token = encodedToken });

        await emailSender.SendConfirmationLinkAsync(
            user,
            user.Email,
            $"Please confirm your email by clicking this link: {confirmationLink}");

        return Results.Ok("Confirmation email sent");
    }

    private static async Task<IResult> ForgotPassword(
        UserManager<User> userManager,
        IEmailSender emailSender,
        ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Results.Ok(); 

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await emailSender.SendEmailAsync(
            user.Email,
            "Reset your password",
            $"Reset your password with this token: {token}");

        return Results.Ok("If the email exists, password reset instructions have been sent.");
    }

    private static async Task<IResult> ResetPassword(
        UserManager<User> userManager,
        ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Results.NotFound();

        var result = await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
        if (!result.Succeeded) return Results.BadRequest(result.Errors);

        return Results.Ok("Password reset successful");
    }
    
}