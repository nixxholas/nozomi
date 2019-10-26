using Microsoft.Extensions.Logging;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseService<T, TContext>
        where T : class
        where TContext : IDbContext
    {
        protected readonly ILogger<T> _logger;
        protected readonly IUnitOfWork<TContext> _unitOfWork;
        protected readonly string _serviceName;

        public BaseService(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceName = typeof(T).FullName;
        }
    }
}
