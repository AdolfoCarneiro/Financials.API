using Financials.Infrastructure.Context;
using Financials.Infrastructure.Repositorio.Interfaces;
using System.Linq.Expressions;

namespace Financials.Infrastructure.Repositorio.Implementacoes
{
    public class RepositorioGenerico<T>(FinancialsDbContext dbContext) : IRepositorioGenerico<T> where T : class
    {
        public FinancialsDbContext _dbContext { get; set; } = dbContext;

        public async Task<T> GetById(Guid id)
        {
            var entidade = await _dbContext
                .Set<T>()
                .FindAsync(id);
            return entidade;
        }
        public async Task<T> Insert(T entidade)
        {
            await _dbContext.Set<T>().AddAsync(entidade);
            return entidade;
        }
        public async Task Insert(List<T> entidades)
        {
            await _dbContext.Set<T>().AddRangeAsync(entidades);
        }
        public void Remove(T entidade)
        {
            _dbContext.Set<T>().Remove(entidade);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entidade = await GetById(id);
            _dbContext.Set<T>().Remove(entidade);
        }

        public T Update(T entidade)
        {
            _dbContext.Update(entidade);
            return entidade;
        }

        public async Task<T> Update(Guid id, T entidade)
        {
            var entidadeExistente = await _dbContext
                .Set<T>()
                .FindAsync(id);

            _dbContext.Entry(entidadeExistente).CurrentValues.SetValues(entidade);
            return entidade;
        }

        public void Update(List<T> entidades)
        {
            entidades.ForEach(e => { _dbContext.Update(e); });
        }

        public int Count()
        {
            var quantidade = _dbContext.Set<T>().Count();
            return quantidade;
        }

        public IQueryable<T> GetByExpression(Expression<Func<T, bool>> filter)
        {
            var entidades = _dbContext
                .Set<T>()
                .Where(filter);

            return entidades;
        }
    }
}