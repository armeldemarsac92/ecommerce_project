using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tdev702.Contracts.Config;
using Tdev702.Repository.Config;
using Tdev702.Repository.Repository;
using Tdev702.Repository.SQL;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.DI;

public static class DbExtensions
{
    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = configuration.GetSection("database").Get<DatabaseConfiguration>() ?? throw new InvalidOperationException("Database configuration not found");
        var cert = new X509Certificate2(Convert.FromBase64String(databaseConfiguration.SslCert));

        DapperMappingConfiguration.ConfigureMappings();
        services.AddSingleton<INpgsqlExceptionHandler, NpgsqlExceptionHandler>();
        services.AddNpgsqlDataSource(databaseConfiguration.DbConnectionString, dataSourceBuilder =>
        {
            dataSourceBuilder.UseVector();
            dataSourceBuilder.UseClientCertificate(cert);
        });
        services.AddTransient<IDBSQLCommand, DbsqlCommand>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITagLinkRepository, TagLinkRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IIdentityRepository<ApplicationUser>, IdentityRepository<ApplicationUser>>();
        services.AddScoped<ITagRepository, TagRepository>();

        return services;
    }
}