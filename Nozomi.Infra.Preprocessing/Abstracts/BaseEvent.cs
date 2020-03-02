using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts.Interfaces;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseEvent<T, TContext>
        where T : class
        where TContext : DbContext
    {
        public const string EventName = nameof(T);
        protected ILogger<T> _logger;
        protected TContext _context;
        protected readonly string _eventName;

        public BaseEvent(ILogger<T> logger, TContext context)
        {
            _logger = logger;
            _context = context;
            _eventName = typeof(T).FullName;
        }
    }
    
    public abstract class BaseEvent<T, TContext, TEntity> : IBaseEvent<TEntity>
        where T : class
        where TEntity : class
        where TContext : DbContext
    {
        public const string EventName = nameof(T);
        protected ILogger<T> _logger;
        protected TContext _context;
        protected readonly string _eventName;

        protected BaseEvent(ILogger<T> logger, TContext context)
        {
            _logger = logger;
            _context = context;
            _eventName = typeof(T).Name;
        }

        public long QueryCount(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return long.MinValue;
            
            return _context.Set<TEntity>()
                .AsEnumerable()
                .Where(predicate.Compile())
                .LongCount();
        }
    }
}