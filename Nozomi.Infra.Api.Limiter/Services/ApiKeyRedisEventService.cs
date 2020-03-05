using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class ApiKeyRedisEventService : BaseService<ApiKeyRedisEventService, AuthDbContext>, 
        IApiKeyRedisEventService
    {
        public ApiKeyRedisEventService(ILogger<ApiKeyRedisEventService> logger, AuthDbContext context) 
            : base(logger, context)
        {
        }

        public ApiKeyRedisEventService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyRedisEventService> logger, 
            AuthDbContext context) : base(contextAccessor, logger, context)
        {
        }
    }
}