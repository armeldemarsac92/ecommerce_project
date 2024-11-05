using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.SQL;

public interface IDBSQLCommand
{
    Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default);

    Task<T> ExecuteAndRetrieveAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T> QueryFirstAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default);

    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);
}

public class DbsqlCommand : IDBSQLCommand
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly INpgsqlExceptionHandler _exceptionHandler;
    private readonly ILogger<DbsqlCommand> _logger;

    public DbsqlCommand(
        NpgsqlDataSource dataSource,
        INpgsqlExceptionHandler exceptionHandler,
        ILogger<DbsqlCommand> logger)
    {
        _dataSource = dataSource;
        _exceptionHandler = exceptionHandler;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing command: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await using var connection = await CreateOpenConnectionAsync(cancellationToken);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var result =
                await connection.ExecuteAsync(new CommandDefinition(sql, parameters, tx,
                    cancellationToken: cancellationToken));
            await tx.CommitAsync(cancellationToken);

            stopwatch.Stop();
            _logger.LogInformation("Command executed in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing command after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            await tx.RollbackAsync(cancellationToken);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T> ExecuteAndRetrieveAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing command: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await using var connection = await CreateOpenConnectionAsync(cancellationToken);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var result =
                await connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters, tx,
                    cancellationToken: cancellationToken));
            await tx.CommitAsync(cancellationToken);

            stopwatch.Stop();
            _logger.LogInformation("Command executed in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing command after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            await tx.RollbackAsync(cancellationToken);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            var result =
                await connection.QueryAsync<T>(new CommandDefinition(sql, parameters,
                    cancellationToken: cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Query executed in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);

            return result.AsList();
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing query after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query first: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            var result =
                await connection.QueryFirstAsync<T>(new CommandDefinition(sql, parameters,
                    cancellationToken: cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Query first executed in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing query first after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query first or default: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            var result =
                await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                    cancellationToken: cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Query first or default executed in {ElapsedMilliseconds}ms.",
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing query first or default after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query single: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            var result =
                await connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters,
                    cancellationToken: cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Query single executed in {ElapsedMilliseconds}ms.", stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing query single after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query single or default: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            var result =
                await connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
                    cancellationToken: cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Query single or default executed in {ElapsedMilliseconds}ms.",
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing query single or default after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    private async ValueTask<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
            stopwatch.Stop();
            _logger.LogInformation("Database connection established in {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            return connection;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to establish database connection after {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<T> CommitAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing transaction with query: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await using var connection = await CreateOpenConnectionAsync(cancellationToken);
        await using var tx = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var result =
                await connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters, tx,
                    cancellationToken: cancellationToken));
            await tx.CommitAsync(cancellationToken);

            stopwatch.Stop();
            _logger.LogInformation("Transaction executed in {ElapsedMilliseconds}ms. SQL: {Sql}",
                stopwatch.ElapsedMilliseconds, sql);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Error executing transaction after {ElapsedMilliseconds}ms. SQL: {Sql}, Parameters: {params}",
                stopwatch.ElapsedMilliseconds, sql, parameters);
            await tx.RollbackAsync(cancellationToken);
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }
}