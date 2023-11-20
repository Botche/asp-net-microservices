namespace Ordering.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Ordering.Application.Contracts.Persistence;
    using Ordering.Domain.Common;
    using Ordering.Infrastructure.Persistence;

    public class RepositoryBase<T> : IAsyncRepository<T>
        where T : EntityBase
    {
        public RepositoryBase(OrderContext context)
        {
            this.Context = context;
        }

        protected OrderContext Context { get; }

        public async Task<T> AddAsync(T entity)
        {
            this.Context.Set<T>().Add(entity);
            await this.Context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            this.Context.Set<T>().Remove(entity);
            await this.Context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await this.Context.Set<T>()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Context.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = this.Context.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = this.Context.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this.Context.Set<T>()
                .FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            this.Context.Entry(entity).State = EntityState.Modified;
            await this.Context.SaveChangesAsync();
        }
    }
}
