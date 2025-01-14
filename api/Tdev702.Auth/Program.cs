using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tdev702.Auth.Database;
using Tdev702.Auth.Extensions;
using Tdev702.Auth.Services;
using Tdev702.AWS.SDK.DI;
using Tdev702.AWS.SDK.SecretsManager;
using Tdev702.Contracts.Config;

var builder = WebApplication.CreateBuilder(args);
builder.AddAwsConfiguration(SecretType.Database, SecretType.Auth);
var databaseConfiguration = builder.Configuration.GetSection("database").Get<DatabaseConfiguration>() ?? throw new InvalidOperationException("Database configuration not found");
var connectionString = databaseConfiguration.DbConnectionString;

builder.Services.AddSEService();
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
builder.Services.AddDistributedMemoryCache();

builder.Services.AddIdentity<User, Role>(options =>
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

builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
        options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
    })
    .AddBearerToken(IdentityConstants.BearerScheme)
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

builder.Services.AddAntiforgery(options => 
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = false; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});


builder.Services.AddSecurityPolicies();


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


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
app.UseHttpsRedirection(); 
app.UseCors("AllowAll");
app.UseAuthentication(); // Must be added
app.UseAuthorization();

app.MapApiEndpoints();

// app.MapIdentityApi<User>();

app.Run();