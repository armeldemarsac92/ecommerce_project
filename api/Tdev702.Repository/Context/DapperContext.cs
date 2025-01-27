using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.Context;

public class DapperContext : IDbContext, IUnitOfWork
{
    private readonly NpgsqlDataSource _dataSource;
    private NpgsqlConnection? _connection;
    private NpgsqlTransaction? _transaction;
    private readonly ILogger<DapperContext> _logger;
    private readonly INpgsqlExceptionHandler _exceptionHandler;

    public DapperContext(NpgsqlDataSource dataSource, ILogger<DapperContext> logger, INpgsqlExceptionHandler exceptionHandler)
    {
        _dataSource = dataSource;
        _logger = logger;
        _exceptionHandler = exceptionHandler;
    }
    
    private async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken ct = default)
    {
        if (_connection != null) return _connection;
    
        var stopwatch = Stopwatch.StartNew();
        try
        {
            _connection = await _dataSource.OpenConnectionAsync(ct);
            stopwatch.Stop();
            _logger.LogInformation("Database Connection established in {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            return _connection;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to establish database Connection after {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _connection ??= await CreateOpenConnectionAsync(ct);
        _transaction = await _connection.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            await _transaction.CommitAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(ct);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing command: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.ExecuteAsync(new CommandDefinition(sql, parameters, _transaction,
                    cancellationToken: cancellationToken));

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
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<T> ExecuteAndRetrieveAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing command: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters, _transaction,
                    cancellationToken: cancellationToken));

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
            throw _exceptionHandler.HandleException(ex, sql, parameters, stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing query: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QueryAsync<T>(new CommandDefinition(sql, parameters,
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
        
        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QueryFirstAsync<T>(new CommandDefinition(sql, parameters,
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

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
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

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters,
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

        await CreateOpenConnectionAsync(cancellationToken);
        try
        {
            var result =
                await _connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
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
}