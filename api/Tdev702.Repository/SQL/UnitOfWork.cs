using System.Data;
using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.SQL;

public interface IUnitOfWork : IDisposable
{
    public Task BeginAsync(CancellationToken ct = default);
    public Task CommitAsync(CancellationToken ct = default);
    public Task RollbackAsync(CancellationToken ct = default);
    public Task<int> ExecuteAsync(string sql, object? parameters = null, 
        CancellationToken cancellationToken = default);
    
    public Task<T> ExecuteAndRetrieveAsync<T>(string sql, object? parameters = null, 
        CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    public Task<T> QueryFirstAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);
    public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    public Task<T> QuerySingleAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    public  Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly INpgsqlExceptionHandler _exceptionHandler;
    private readonly ILogger<DbsqlCommand> _logger;
    private NpgsqlConnection? _connection;
    private NpgsqlTransaction? _transaction;

    public UnitOfWork(
        NpgsqlDataSource dataSource, 
        INpgsqlExceptionHandler exceptionHandler, 
        ILogger<DbsqlCommand> logger)
    {
        _dataSource = dataSource;
        _exceptionHandler = exceptionHandler;
        _logger = logger;
    }

    public NpgsqlConnection Connection => _connection ?? throw new InvalidOperationException("Connection not initialized");
    public NpgsqlTransaction Transaction => _transaction ?? throw new InvalidOperationException("Transaction not initialized");

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        _connection = await CreateOpenConnectionAsync(ct);
        _transaction = await _connection.BeginTransactionAsync(ct);
    }

    public async Task BeginAsync(CancellationToken ct = default)
    {
        await InitializeAsync(ct);
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
    
    
    public async Task<int> ExecuteAsync(string sql, object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing command: {Sql} and parameters: {params}", sql, parameters);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result =
                await Connection.ExecuteAsync(new CommandDefinition(sql, parameters, Transaction,
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

        try
        {
            var result =
                await Connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters, Transaction,
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

        try
        {
            var result =
                await Connection.QueryAsync<T>(new CommandDefinition(sql, parameters,
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
            var result =
                await Connection.QueryFirstAsync<T>(new CommandDefinition(sql, parameters,
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
            var result =
                await Connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
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
            var result =
                await Connection.QuerySingleAsync<T>(new CommandDefinition(sql, parameters,
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
            var result =
                await Connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, parameters,
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
    
    private async ValueTask<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken =  default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
            stopwatch.Stop();
            _logger.LogInformation("Database Connection established in {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            return connection;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to establish database Connection after {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public void Dispose()
    {
        Connection.Dispose();
        Transaction.Dispose();
    }
}