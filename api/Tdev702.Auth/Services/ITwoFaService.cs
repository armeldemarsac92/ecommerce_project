using Microsoft.AspNetCore.Identity;
using Tdev702.Contracts.Auth;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;

namespace Tdev702.Auth.Services;

public interface ITwoFaService
{
    public Task VerifyCodeAsync(User user, string code);
    public Task<string> GenerateCodeAsync(User user);
}

public class TwoFaService : ITwoFaService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<TwoFaService> _logger;
    
    public TwoFaService(
        UserManager<User> userManager, 
        ILogger<TwoFaService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task VerifyCodeAsync(User user, string code)
    {
        _logger.LogInformation("Verifying user code for {UserId}", user.Id);

        string providerName;
        switch (user.PreferredTwoFactorProvider)
        {
            case TwoFactorType.Email:
                _logger.LogInformation("Using email provider");
                providerName = TokenOptions.DefaultEmailProvider;
                break;
    
            case TwoFactorType.SMS:
                _logger.LogInformation("Using SMS provider");
                providerName = TokenOptions.DefaultPhoneProvider;
                break;
    
            default:
                _logger.LogError("Unknown provider type: {provider}", user.PreferredTwoFactorProvider);
                throw new BadRequestException("Invalid two-factor provider configured.");
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, providerName, code);
        if (!isValid)
        {
            await _userManager.AccessFailedAsync(user);
            _logger.LogWarning("Invalid verification code for user {UserId}", user.Id);
            throw new BadRequestException("Invalid code.");
        }
    }

    public async Task<string> GenerateCodeAsync(User user)
    {
        _logger.LogInformation("Generating user code for {UserId}", user.Id);

        string providerName;
        switch (user.PreferredTwoFactorProvider)
        {
            case TwoFactorType.Authenticator:
                _logger.LogInformation("Using authenticator provider");
                providerName = TokenOptions.DefaultAuthenticatorProvider;
                break;
        
            case TwoFactorType.Email:
                _logger.LogInformation("Using email provider");
                providerName = TokenOptions.DefaultEmailProvider;
                break;
        
            case TwoFactorType.SMS:
                _logger.LogInformation("Using SMS provider");
                providerName = TokenOptions.DefaultPhoneProvider;
                break;
        
            default:
                _logger.LogError("Unknown provider type: {provider}", user.PreferredTwoFactorProvider);
                throw new BadRequestException("Invalid two-factor provider configured.");
        }

        return await _userManager.GenerateTwoFactorTokenAsync(user, providerName);
    }
}