using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tdev702.Auth.Database;
using Tdev702.Auth.DI;
using Tdev702.Auth.Extensions;
using Tdev702.Auth.Middlewares.ExceptionHandlers;
using Tdev702.Auth.Services;
using Tdev702.AWS.SDK.DI;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;
using Tdev702.Contracts.Exceptions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Auth);
var databaseConfiguration = builder.Configuration.GetSection("database").Get<DatabaseConfiguration>() ?? throw new InvalidOperationException("Database configuration not found");
var connectionString = databaseConfiguration.DbConnectionString;
var authConfiguration = builder.Configuration.GetSection("auth").Get<AuthConfiguration>() ?? throw new InvalidOperationException("Auth configuration not found");

var services = builder.Services;
services.AddSEService();
services.AddDistributedMemoryCache();
services.AddEndpointsApiExplorer();
services.AddAuthServices();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    options.Tokens.AuthenticatorIssuer = "Epitech Project";
    
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<Role>()
.AddDefaultTokenProviders()
.AddApiEndpoints();

services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
        options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.SigninKey!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = authConfiguration.GoogleClientId;
        options.ClientSecret = authConfiguration.GoogleClientSecret;

    })
    .AddFacebook(options =>
    {
        options.ClientId = authConfiguration.FacebookAppId;
        options.ClientSecret = authConfiguration.FacebookAppSecret;
    });

services.AddAntiforgery(options => 
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = false; 
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