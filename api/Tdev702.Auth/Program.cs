using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tdev702.Auth.Extensions;
using Tdev702.Auth.Middlewares.ExceptionHandlers;
using Tdev702.Auth.Services;
using Tdev702.AWS.SDK.DI;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;
using Tdev702.Contracts.Database;
using Tdev702.Contracts.Exceptions;
using Tdev702.Stripe.SDK.DI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Auth, SecretType.Stripe);
var databaseConfiguration = builder.Configuration.GetSection("database").Get<DatabaseConfiguration>() ?? throw new InvalidOperationException("Database configuration not found");
var connectionString = databaseConfiguration.DbConnectionString;
var authConfiguration = builder.Configuration.GetSection("auth").Get<AuthConfiguration>() ?? throw new InvalidOperationException("Auth configuration not found");
var stripeConfiguration = builder.Configuration.GetSection("stripe").Get<StripeConfiguration>()?? throw new InvalidOperationException("Stripe configuration not found");
var services = builder.Services;
services.AddSEService();
services.AddDistributedMemoryCache();
services.AddStripeServices(stripeConfiguration);
services.AddMessaging();
services.AddEndpointsApiExplorer();
services.AddAuthServices();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

services.AddSwagger("Auth Server");
services.AddIdentity();
services.AddAuth(authConfiguration);
services.AddAntiforgery(options => 
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});

services.AddSecurityPolicies();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
services.AddProblemDetails();
services.AddExceptionHandler<BadRequestExceptionHandler>();
services.AddExceptionHandler<ConflictExceptionHandler>();
services.AddExceptionHandler<NotFoundExceptionHandler>();
services.AddExceptionHandler<DatabaseExceptionHandler>();
services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection(); 
app.UseCors("AllowAll");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapApiEndpoints();

app.Run();