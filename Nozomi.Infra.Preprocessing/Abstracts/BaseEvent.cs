using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Interfaces;
using Nozomi.Preprocessing.Abstracts.Interfaces;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseEvent<T, TContext>
        where T : class
        where TContext : IDbContext
    {
        public const string EventName = nameof(T);
        protected ILogger<T> _logger;
        protected IUnitOfWork<TContext> _unitOfWork;

        public BaseEvent(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
    
    public abstract class BaseEvent<T, TContext, TEntity> : IBaseEvent<TEntity>
        where T : class
        where TEntity : class
        where TContext : IDbContext
    {
        public const string EventName = nameof(T);
        protected ILogger<T> _logger;
        protected IUnitOfWork<TContext> _unitOfWork;

        public BaseEvent(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public long QueryCount(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return long.MinValue;
            
            return _unitOfWork.GetRepository<TEntity>()
                .GetQueryable().AsEnumerable()
                .Where(predicate.Compile())
                .LongCount();
        }
    }
}