using System;
using Microsoft.Extensions.Logging;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;

namespace Nozomi.Service.Services
{
    public abstract class BaseService<T, TContext>
        where T : class
        where TContext : IDbContext
    {
        protected ILogger<T> _logger;
        protected IUnitOfWork<TContext> _unitOfWork;

        public BaseService(ILogger<T> logger, IUnitOfWork<TContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}
