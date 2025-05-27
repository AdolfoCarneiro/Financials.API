using Financials.Infrastructure.Context;

namespace Financials.Infrastructure.Repositorio.Interfaces;
public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync();
    FinancialsDbContext DbContext { get; }
}
