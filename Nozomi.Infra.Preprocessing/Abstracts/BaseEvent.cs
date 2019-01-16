using Microsoft.Extensions.Logging;
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
}