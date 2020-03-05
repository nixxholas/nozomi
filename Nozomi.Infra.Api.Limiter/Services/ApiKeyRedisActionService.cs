using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class ApiKeyRedisActionService : BaseService<ApiKeyRedisActionService, AuthDbContext>, 
        IApiKeyRedisActionService
    {
        public ApiKeyRedisActionService(ILogger<ApiKeyRedisActionService> logger, AuthDbContext context) 
            : base(logger, context)
        {
        }

        public ApiKeyRedisActionService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyRedisActionService> logger, 
            AuthDbContext context) : base(contextAccessor, logger, context)
        {
        }
    }
}