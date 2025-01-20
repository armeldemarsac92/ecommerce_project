using Tdev702.Api.DI;
using Tdev702.Api.Extensions;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;
using Tdev702.Repository.DI;
using Tdev702.Stripe.SDK.DI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Stripe, SecretType.Auth);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var services = builder.Services;

services.AddDbConnection(builder.Configuration);

var stripeConfiguration = builder.Configuration.GetSection("stripe").Get<StripeConfiguration>() ?? throw new InvalidOperationException("Stripe configuration not found");
var authConfiguration = builder.Configuration.GetSection("auth").Get<AuthConfiguration>()?? throw new InvalidOperationException("Auth configuration not found");
services.AddStripeServices(stripeConfiguration); 
services.AddApiServices(builder.Configuration); 
services.AddAuth(authConfiguration);
services.AddSecurityPolicies();
builder.Services.AddEndpointsApiExplorer();
services.AddSwagger("API");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
