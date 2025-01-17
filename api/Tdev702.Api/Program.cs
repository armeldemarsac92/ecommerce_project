using Microsoft.OpenApi.Models;
using Tdev702.Api.DI;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;
using Tdev702.Repository.DI;
using Tdev702.Stripe.SDK.DI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var services = builder.Services;

services.AddDbConnection(builder.Configuration);

var stripeConfiguration = builder.Configuration.GetSection("stripe").Get<StripeConfiguration>() ?? throw new InvalidOperationException("Stripe configuration not found");
services.AddStripeServices(stripeConfiguration); 
services.AddApiServices(builder.Configuration); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tdev702 API", Version = "v1" });
    
    // Handle naming conflicts with product and stripe product schema
    c.CustomSchemaIds(type => 
    {
        if (type.Namespace?.StartsWith("Stripe") == true)
        {
            return $"Stripe{type.Name}";
        }
        return type.Name;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
