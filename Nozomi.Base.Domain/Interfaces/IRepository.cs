using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nozomi.Data.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> Get();
        IQueryable<T> GetQueryable();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
    }
}