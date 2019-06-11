using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts.Interfaces;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseEvent<T, TContext>
        where T : class
        where TContext : IDbContext
    {
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
        protected ILogger<T> _logger;
        protected IUnitOfWork<TContext> _unitOfWork;

        public BaseEvent(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public long QueryCount(Func<T, bool> condition)
        {
            if (condition == null) return long.MinValue;
            
            return _unitOfWork.GetRepository<T>()
                .GetQueryable().AsEnumerable()
                .Where(condition)
                .LongCount();
        }

        public long QueryCount(Func<TEntity, bool> predicate)
        {
            if (predicate == null) return long.MinValue;
            
            return _unitOfWork.GetRepository<TEntity>()
                .GetQueryable().AsEnumerable()
                .Where(predicate)
                .LongCount();
        }
    }
}