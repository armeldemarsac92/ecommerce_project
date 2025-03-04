using Serilog;
using Serilog.Debugging;
using Tdev702.Api.DI;
using Tdev702.Api.Extensions;
using Tdev702.Api.Middlewares.ExceptionHandlers;
using Tdev702.AWS.SDK.CloudWatch;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;
using Tdev702.OpenFoodFact.SDK.Extensions;
using Tdev702.Repository.DI;
using Tdev702.Stripe.SDK.DI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Stripe, SecretType.Auth);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

Log.Logger = new LoggerConfiguration()
    .ConfigureSerilog(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
SelfLog.Enable(Console.Error);   

var services = builder.Services;

services.AddDbConnection(builder.Configuration);

var stripeConfiguration = builder.Configuration.GetSection("stripe").Get<StripeConfiguration>() ?? throw new InvalidOperationException("Stripe configuration not found");
var authConfiguration = builder.Configuration.GetSection("auth").Get<AuthConfiguration>()?? throw new InvalidOperationException("Auth configuration not found");
services.AddOpenFoodFactServices();
services.AddStripeServices(stripeConfiguration); 
services.AddApiServices(builder.Configuration); 
services.AddAuth(authConfiguration);
services.AddSecurityPolicies();
services.AddMessaging();
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(authConfiguration.CorsAllowOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

services.AddEndpointsApiExplorer();
services.AddSwagger("API");

services.AddProblemDetails();
services.AddExceptionHandler<BadRequestExceptionHandler>();
services.AddExceptionHandler<ConflictExceptionHandler>();
services.AddExceptionHandler<NotFoundExceptionHandler>();
services.AddExceptionHandler<DatabaseExceptionHandler>();
services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build(); 

if (app.Environment.IsStaging() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();   
app.UseAuthorization();

app.MapApiEndpoints();

app.Run();
