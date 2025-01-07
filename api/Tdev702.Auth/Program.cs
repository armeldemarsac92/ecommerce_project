using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tdev702.AWS.SDK;
using Tdev702.Auth.Database;
using Tdev702.Auth.Extensions;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Auth);
var databaseConfiguration = builder.Configuration.GetSection("database").Get<DatabaseConfiguration>() ?? throw new InvalidOperationException("Database configuration not found");
var connectionString = databaseConfiguration.DbConnectionString;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAntiforgery(options => 
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = false; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<Role>()
.AddDefaultTokenProviders()
.AddApiEndpoints();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax; 
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme, options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromHours(1);
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["auth:googleclientid"]!;
        options.ClientSecret = builder.Configuration["auth:googleclientsecret"]!;
        options.CallbackPath = "/signin-google";
    })
    .AddFacebook(options =>
    {
        options.ClientId = builder.Configuration["auth:facebookappid"]!;
        options.ClientSecret = builder.Configuration["auth:facebookappsecret"]!;
        options.CallbackPath = "/signin-facebook";
    });


builder.Services.AddAuthorizationBuilder();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSecurityPolicies();

var app = builder.Build();

app.MapApiEndpoints();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthentication(); // Must be added
app.UseAuthorization();

app.UseHttpsRedirection(); 

app.Run();