using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Services.ApiKey
{
    public class ApiKeyService : BaseService<ApiKeyService, AuthDbContext>, IApiKeyService
    {
        public ApiKeyService(ILogger<ApiKeyService> logger, AuthDbContext context) : base(logger, context)
        {
        }

        public ApiKeyService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyService> logger, 
            AuthDbContext context) : base(contextAccessor, logger, context)
        {
        }
    }
}