using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tdev702.Repository.Utils;

namespace Tdev702.Repository.Context;

public interface IDbContext
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<T> QuerySingleAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken ct = default);
}