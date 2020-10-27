using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities.Base;
using Ordering.Core.Repos.Base;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repos.Base
{
    public class Repo<T> : IRepo<T> where T : Entity
    {
        protected readonly OrderContext _dbContext;

        public Repo(OrderContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> pred)
        {
            return await _dbContext.Set<T>().Where(pred).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> pred = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> q = _dbContext.Set<T>();
            if (disableTracking)
                q = q.AsNoTracking();
            if (string.IsNullOrWhiteSpace(includeString))
                q = q.Include(includeString);
            if (pred != null)
                q = q.Where(pred);
            if (orderBy != null)
                return await orderBy(q).ToListAsync();
            return await q.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> pred = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> q = _dbContext.Set<T>();
            if (disableTracking)
                q = q.AsNoTracking();
            if (includes != null)
                q = includes.Aggregate(q, (c, i) => c.Include(i));
            if (pred != null)
                q = q.Where(pred);
            if (orderBy != null)
                return await orderBy(q).ToListAsync();
            return await q.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
