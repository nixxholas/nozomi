using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nozomi.Preprocessing.Abstracts
{
    public abstract class BaseService<T, TContext>
        where T : class
        where TContext : DbContext
    {
        // https://stackoverflow.com/questions/38571032/how-to-get-httpcontext-current-in-asp-net-core
        private readonly IHttpContextAccessor _contextAccessor;
        protected readonly ILogger<T> _logger;
        protected readonly TContext _context;
        protected readonly string _serviceName;

        public BaseService(ILogger<T> logger, TContext context)
        {
            _logger = logger;
            _context = context;
            _serviceName = typeof(T).FullName;
        }

        public BaseService(IHttpContextAccessor contextAccessor, ILogger<T> logger, TContext context)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _context = context;
            _serviceName = typeof(T).FullName;
        }

        public IHttpContextAccessor CurrentAccessor()
        {
            return _contextAccessor;
        }
    }
}
