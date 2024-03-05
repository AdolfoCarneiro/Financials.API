using Financials.Infrastructure.Context;

namespace Financials.Infrastructure.Repositorio
{
    public class FinancialsRepositorio(FinancialsDbContext financialsDbContext) : IFInancialsRepositorio
    {
        private readonly FinancialsDbContext _dbContext = financialsDbContext;

        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
