using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tdev702.Repository.Brands;
using Tdev702.Contracts.API.Auth;
using Tdev702.Repository.Config;
using Tdev702.Repository.Repository;
using Tdev702.Repository.SQL;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.DI;

public static class DbExtensions
{
    public static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               throw new InvalidOperationException(
                                   "DB_CONNECTION_STRING not found in environment variables");
        
        // var certificatePath = Environment.GetEnvironmentVariable("SUPABASE_SSL_CERT_PATH") ??
        //                        throw new InvalidOperationException(
        //                            "SUPABASE_SSL_CERT_PATH not found in environment variables");
        //
        // var cert = new X509Certificate2(certificatePath);

        DapperMappingConfiguration.ConfigureMappings();
        services.AddSingleton<INpgsqlExceptionHandler, NpgsqlExceptionHandler>();
        services.AddNpgsqlDataSource(connectionString, dataSourceBuilder =>
        {
            dataSourceBuilder.UseVector();
            // dataSourceBuilder.UseClientCertificate(cert);
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