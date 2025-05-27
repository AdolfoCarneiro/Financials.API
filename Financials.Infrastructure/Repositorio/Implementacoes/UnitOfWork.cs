using Financials.Infrastructure.Context;
using Financials.Infrastructure.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Financials.Infrastructure.Repositorio.Implementacoes;
public class UnitOfWork(FinancialsDbContext dbContext) : IUnitOfWork, IDisposable
{
    private readonly FinancialsDbContext _dbContext = dbContext;
    private IDbContextTransaction _transaction;
    private bool _disposed = false;

    public FinancialsDbContext DbContext => _dbContext;

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
        if (_transaction is not null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }

    public Task<int> SaveChangesAsync()
        => _dbContext.SaveChangesAsync();

    ~UnitOfWork()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
        _disposed = true;
    }
}
