using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Tdev702.Repository.Context;

public interface IUnitOfWork : IDisposable
{
    public Task BeginTransactionAsync(CancellationToken ct = default);
    public Task CommitAsync(CancellationToken ct = default);
    public Task RollbackAsync(CancellationToken ct = default);
}