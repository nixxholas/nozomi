using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nozomi.Data.Interfaces;

namespace Nozomi.Repo.BCL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(IDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            var entry = _dbSet.Add(entity);

        }

        public void Delete(T entity)
        {
            T existing = _dbSet.Find(entity);
            if (existing != null) _dbSet.Remove(existing);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            T existing = _dbSet.SingleOrDefault<T>(predicate);
            if (existing != null) _dbSet.Remove(existing);
        }

        public IEnumerable<T> Get()
        {
            return _dbSet.AsEnumerable<T>();
        }

        public IQueryable<T> GetQueryable() 
        {
            return _dbSet.AsQueryable<T>();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsEnumerable<T>();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable<T>();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Update(params T[] entities) => _dbSet.UpdateRange(entities);


        public void Update(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}