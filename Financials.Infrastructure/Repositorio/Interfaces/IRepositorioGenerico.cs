using System.Linq.Expressions;

namespace Financials.Infrastructure.Repositorio.Interfaces
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<T> Insert(T entidade);
        Task Insert(List<T> entidades);
        void Remove(T entidade);
        Task RemoveAsync(Guid id);
        T Update(T entidade);
        Task<T> Update(Guid id, T entidade);
        void Update(List<T> entidades);
        int Count();
        IQueryable<T> GetByExpression(Expression<Func<T, bool>> filter);
    }
}