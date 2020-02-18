using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseService<T>
        where T : class
    {
        // https://stackoverflow.com/questions/38571032/how-to-get-httpcontext-current-in-asp-net-core
        private readonly IHttpContextAccessor _contextAccessor;
        protected readonly ILogger<T> _logger;
        protected readonly string _serviceName;

        public BaseService(ILogger<T> logger)
        {
            _logger = logger;
            _serviceName = typeof(T).FullName;
        }

        public BaseService(IHttpContextAccessor contextAccessor, ILogger<T> logger)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _serviceName = typeof(T).FullName;
        }

        public IHttpContextAccessor CurrentAccessor()
        {
            return _contextAccessor;
        }
    }
    
    public abstract class BaseService<T, TContext>
        where T : class
        where TContext : IDbContext
    {
        // https://stackoverflow.com/questions/38571032/how-to-get-httpcontext-current-in-asp-net-core
        private readonly IHttpContextAccessor _contextAccessor;
        protected readonly ILogger<T> _logger;
        protected readonly IUnitOfWork<TContext> _unitOfWork;
        protected readonly string _serviceName;

        public BaseService(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceName = typeof(T).FullName;
        }

        public BaseService(IHttpContextAccessor contextAccessor, ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _serviceName = typeof(T).FullName;
        }

        public IHttpContextAccessor CurrentAccessor()
        {
            return _contextAccessor;
        }
    }
}
