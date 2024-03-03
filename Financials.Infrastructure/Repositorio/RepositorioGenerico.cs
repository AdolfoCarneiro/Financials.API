using Financials.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Financials.Infrastructure.Repositorio
{
    public class RepositorioGenerico<T>(FinancialsDbContext dbContext) where T : class
    {
        public FinancialsDbContext _dbContext { get; set; } = dbContext;

        public virtual IEnumerable<T> GetPage(
               Expression<Func<T, bool>> filter = null,
               Expression<Func<T, object>> orderByDescending = null,
               Expression<Func<T, object>> orderBy = null,
               string includeProperties = "",
               int page = 1,
               int take = 50)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var skip = page <= 1 ? 0 : (page - 1) * take;
            if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending).Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
            else
            {
                query = query.Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
        }
        public List<T> GetAll()
        {
            IQueryable<T> result = _dbContext.Set<T>();
            return result.ToList();
        }
        public IQueryable<T> GetAllQueryable()
        {
            IQueryable<T> result = _dbContext.Set<T>();
            return result;
        }
        public List<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }
        public async Task<T> GetById(Guid id, string[] includeProperties)
        {
            var keyProperty = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            var parameter = Expression.Parameter(typeof(T), "e");
            var key = Expression.PropertyOrField(parameter, keyProperty?.Name ?? "Id");
            var value = Convert.ChangeType(id, keyProperty?.ClrType ?? typeof(int));
            var predicate = Expression.Lambda<Func<T, bool>>(Expression.Equal(key, Expression.Constant(value)), parameter);

            var query = _dbContext.Set<T>().AsQueryable();

            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<T> Insert(T entidade)
        {
            await _dbContext.Set<T>().AddAsync(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }
        public async Task Insert(List<T> entidades)
        {
            entidades.ForEach(async e => { await _dbContext.Set<T>().AddAsync(e); });
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveAsync(Guid id)
        {
            var entidade = await this.GetById(id);
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Remove(T entidade)
        {
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<T> Update(T entidade)
        {
            _dbContext.Update(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }
        public async Task<T> Update(Guid id, T entidade)
        {
            var entidadeExistente = await _dbContext
                .Set<T>()
                .FindAsync(id);

            _dbContext.Entry(entidadeExistente).CurrentValues.SetValues(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }
        public async Task Update(List<T> entidades)
        {
            entidades.ForEach(e => { _dbContext.Update(e); });
            await _dbContext.SaveChangesAsync();
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
        public virtual IEnumerable<TResult> GetPageLazy<TResult, TEntity>(
           Expression<Func<T, TResult>> selector,
           Expression<Func<T, bool>> filter = null,
           Expression<Func<T, T>> orderBy = null,
           string includeProperties = "",
           int page = 1,
           int take = 50)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var skip = page <= 1 ? 0 : (page - 1) * take;
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).Skip(skip).Take(take);
                return query.AsNoTracking().Select(selector);
            }
            else
            {
                query = query.Skip(skip).Take(take);
                return query.AsNoTracking().Select(selector);
            }
        }
    }
}