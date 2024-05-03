using Financials.Infrastructure.Context;
using System.Linq.Expressions;

namespace Financials.Infrastructure.Repositorio.Interfaces
{
    public interface IRepositorioGenerico<T> where T : class
    {
        FinancialsDbContext _dbContext { get; set; }

        int Count();
        List<T> GetAll();
        List<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAllQueryable();
        IQueryable<T> GetByExpression(Expression<Func<T, bool>> filter);
        Task<T> GetById(Guid id, string[] includeProperties = null);
        IEnumerable<T> GetPage(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderByDescending = null, Expression<Func<T, object>> orderBy = null, string includeProperties = "", int page = 1, int take = 50);
        IEnumerable<TResult> GetPageLazy<TResult, TEntity>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null, Expression<Func<T, T>> orderBy = null, string includeProperties = "", int page = 1, int take = 50);
        Task Insert(List<T> entidades);
        Task<T> Insert(T entidade);
        Task Remove(T entidade);
        Task RemoveAsync(Guid id);
        Task<T> Update(Guid id, T entidade);
        Task Update(List<T> entidades);
        Task<T> Update(T entidade);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}