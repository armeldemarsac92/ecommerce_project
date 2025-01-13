using Tdev702.Api.DI;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Repository.DI;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var services = builder.Services;

services.AddDbConnection(builder.Configuration);
services.AddApiServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
