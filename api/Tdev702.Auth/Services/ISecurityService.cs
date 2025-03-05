﻿using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Tdev702.Contracts.Auth;
using Tdev702.Contracts.Config;

namespace Tdev702.Auth.Services;

public interface ISecurityService
{
    Task StoreAuthState(AuthenticationParameters authParameters);
    Task<AuthStateData> ValidateState(string state);
    bool ValidateIdToken(string idToken, string nonce);
    public Dictionary<string, string> MapTokenToUserDict(string idToken);
}

public class SecurityService : ISecurityService
{
   private readonly IDistributedCache _cache;
   private readonly AuthConfiguration _configuration;
   private readonly ILogger<SecurityService> _logger;

   public SecurityService(
       IDistributedCache cache, 
       AuthConfiguration configuration, 
       HttpClient httpClient, 
       ILogger<SecurityService> logger)
   {
       _cache = cache;
       _configuration = configuration;
       _logger = logger;
   }
   

   public async Task StoreAuthState(AuthenticationParameters authParameters)
   {
       var idpConfig = _configuration.IdentityProviders.FirstOrDefault(idp => idp.Name == authParameters.IdentityProvider);
       if (idpConfig is null) throw new Exception($"Invalid IDP: {authParameters.IdentityProvider}");

       authParameters.FrontEndRedirectUri = idpConfig.FrontEndRedirectUri;
       var stateData = new AuthStateData
       {
           AuthenticationParameters = authParameters,
           Timestamp = DateTime.UtcNow
       };

       await _cache.SetStringAsync(
           $"auth_state:{authParameters.State}",
           JsonSerializer.Serialize(stateData),
           new DistributedCacheEntryOptions
           {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
           }
       );
   }

   public async Task<AuthStateData> ValidateState(string state)
   {
       _logger.LogInformation("Retrieving state data from cache for state: {state} if exists", state);
       var stateJson = await _cache.GetStringAsync($"auth_state:{state}");
       if (string.IsNullOrEmpty(stateJson))
           throw new SecurityException($"Invalid or expired state: {stateJson}");

       var stateData = JsonSerializer.Deserialize<AuthStateData>(stateJson);
       if (DateTime.UtcNow - stateData.Timestamp > TimeSpan.FromMinutes(15))
           throw new SecurityException("State expired");
       await _cache.RemoveAsync($"auth_state:{state}");
       
       _logger.LogInformation("State data validated and retrieved successfully for state: {state}", state);
       return stateData;
   }

   public  bool ValidateIdToken(string idToken, string nonce)
   {
       var handler = new JwtSecurityTokenHandler();
       var token = handler.ReadJwtToken(idToken);

       if (token.Payload.Nonce != nonce)
           throw new SecurityException("Invalid nonce");


       return true;
   }

   public Dictionary<string, string> MapTokenToUserDict(string idToken)
   {
       var handler = new JwtSecurityTokenHandler();
       var jwtToken = handler.ReadJwtToken(idToken);
       var claims = jwtToken.Claims;
       var claimLookup = claims.GroupBy(c => c.Type)
           .ToDictionary(g => g.Key, g => g.First().Value);
       return claimLookup;
   }
}