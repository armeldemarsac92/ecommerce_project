using System.Security.Claims;
using MassTransit;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Tdev702.AWS.SDK.SES;
using Tdev702.Contracts.Auth;
using Tdev702.Contracts.Auth.Request;
using Tdev702.Contracts.Auth.Response;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Auth.Services;

public record UserRecord(
    string FirstName,
    string LastName, 
    string Email, 
    bool EmailConfirmed,
    string? Picture,
    string? Password
);
public interface IUserService
{
    public Task RegisterUserAsync(string userEmail, string userRole);
    public Task<User> ProvisionUserAsync(UserRecord userRecord, string userRole);
    public Task<AccessTokenResponse> LoginUserAsync(LoginUserRequest request);
    public Task<AccessTokenResponse> UpdateUserAsync(ClaimsPrincipal claims, UpdateUserRequest request);
    public Task Send2FaCodeAsync(Get2FaCodeRequest request);
}

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly LinkGenerator _linkGenerator;
    private readonly IEmailSender<User> _emailSender;
    private readonly IEmailService _emailService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITwoFaService _i2FaService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<User> userManager, 
        ILogger<UserService> logger, 
        LinkGenerator linkGenerator, 
        IEmailSender<User> emailSender, 
        IPublishEndpoint publishEndpoint, 
        ITwoFaService i2FaService, 
        ITokenService tokenService, 
        IEmailService emailService)
    {
        _userManager = userManager;
        _logger = logger;
        _linkGenerator = linkGenerator;
        _emailSender = emailSender;
        _publishEndpoint = publishEndpoint;
        _i2FaService = i2FaService;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task RegisterUserAsync(string userEmail, string userRole)
    {
        try
        {
            _logger.LogInformation("Creating user: {Email}", userEmail);
            var user = new User { UserName = userEmail, Email = userEmail, PreferredTwoFactorProvider = TwoFactorType.Email, CreatedAt = DateTime.UtcNow};
            var creationResult = await _userManager.CreateAsync(user);
            CheckResult(creationResult);
            _logger.LogInformation("User created: {UserId}", user.Id);
            _logger.LogInformation("Adding user {UserId} to role: {UserRole}", user.Id, userRole);
            var roleResult = await _userManager.AddToRoleAsync(user, userRole);
            CheckResult(roleResult);
            _logger.LogInformation("User {UserId} added to role: {UserRole}", user.Id, userRole);
            _logger.LogInformation("Publishing user {UserId} to the stripe event bus to generate its stripe id.", user.Id);
            await _publishEndpoint.Publish(user);
            
            _logger.LogInformation("Sending confirmation code to user {UserId}", user.Id);
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var code = await _i2FaService.GenerateCodeAsync(user);
            await _emailService.SendVerificationCodeAsync(user.Email, code);
            _logger.LogInformation("Confirmation code sent to user {UserId}", user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating user: {Message}", ex.Message);
            throw;
        }
    }        
    
    public async Task<User> ProvisionUserAsync(UserRecord userToProvision, string userRole)
    {
        try
        {
            _logger.LogInformation("Creating user: {Email}", userToProvision.Email);
            var user = new User { UserName = userToProvision.Email, Email = userToProvision.Email, FirstName = userToProvision.FirstName, LastName = userToProvision.LastName, ProfilePicture = userToProvision.Picture, CreatedAt = DateTime.UtcNow};
            var creationResult = await _userManager.CreateAsync(user);
            CheckResult(creationResult);
            _logger.LogInformation("User created: {UserId}", user.Id);
            _logger.LogInformation("Adding user {UserId} to role: {UserRole}", user.Id, userRole);
            var roleResult = await _userManager.AddToRoleAsync(user, userRole);
            CheckResult(roleResult);
            _logger.LogInformation("User {UserId} added to role: {UserRole}", user.Id, userRole);
            _logger.LogInformation("Publishing user {UserId} to the stripe event bus to generate its stripe id.", user.Id);
            await _publishEndpoint.Publish(user);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating user: {Message}", ex.Message);
            throw;
        }
    }    

    public async Task<AccessTokenResponse> UpdateUserAsync(ClaimsPrincipal claims, UpdateUserRequest request)
    {
        var user = await _userManager.GetUserAsync(claims);
        CheckUser(user);

        _logger.LogInformation("Updating user: {Email}", user.Email);
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.ProfilePicture = request.ProfilePicture;
        
        var result = await _userManager.UpdateAsync(user);
        CheckResult(result);
        
        return await _tokenService.GetAccessTokenAsync(user);
    }

    public async Task Send2FaCodeAsync(Get2FaCodeRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        CheckUser(user);
        _logger.LogInformation("Sending 2FA code to user {UserId}", user.Id);
        
        
        _logger.LogInformation("Sending confirmation code to user {UserId}", user.Id);
        var code = await _i2FaService.GenerateCodeAsync(user);
        await _emailService.SendVerificationCodeAsync(user.Email, code);
        _logger.LogInformation("Confirmation code sent to user {UserId}", user.Id);
    }

    public async Task<AccessTokenResponse> LoginUserAsync(LoginUserRequest request)
    {
        _logger.LogInformation("Logging in user: {Email}", request.Email);
        var user = await _userManager.FindByEmailAsync(request.Email);
        CheckUser(user);
        _logger.LogInformation("Verifying 2FA code for user {userId}", user.Id);
        await _i2FaService.VerifyCodeAsync(user, request.TwoFactorCode);
        _logger.LogInformation("2FA code verified for user {UserId}", user.Id);
        
        if (user.EmailConfirmed == false)
        {
            _logger.LogInformation("User {UserId} email not confirmed yet.", user.Id);
            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            CheckResult(result);
            _logger.LogInformation("User {UserId} email confirmed.", user.Id);
        }
        
        return await _tokenService.GetAccessTokenAsync(user);
    }
    
    private void CheckUser(User? user)
    {
        if (user == null) throw new BadRequestException("User not found");
        _logger.LogInformation("User {UserId} found.", user.Id);
    }
    
    private void CheckResult(IdentityResult result)
    {
        if (result.Errors.Any())
        {
            _logger.LogError("Operation failed with {ErrorCount} errors.", 
                result.Errors.Count());
    
            var errorMessages = result.Errors.Select(e => e.Description).ToList();
    
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error code {Code}: {Description}", 
                    error.Code, error.Description);
            }
    
            throw new BadRequestException(
                $"Operation failed: {string.Join(", ", errorMessages)}");
        }
    }
}