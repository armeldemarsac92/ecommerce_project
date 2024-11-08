namespace Tdev702.Repository.Config;

public interface IDbsqlConfig
{
    public string ConnectionString { get; }
}

public class DbsqlConfig : IDbsqlConfig
{
    public DbsqlConfig()
    {
        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                           throw new InvalidOperationException(
                               "DB_CONNECTION_STRING not found in environment variables");
    }

    public string ConnectionString { get; set; }
}