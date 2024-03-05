namespace Financials.Infrastructure.Repositorio
{
    public interface IFInancialsRepositorio : IDisposable
    {
        Task CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
