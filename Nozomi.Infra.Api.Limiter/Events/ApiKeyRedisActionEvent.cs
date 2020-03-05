using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class ApiKeyRedisActionEvent : BaseEvent<ApiKeyRedisActionEvent, AuthDbContext>, IApiKeyRedisActionEvent
    {
        public ApiKeyRedisActionEvent(ILogger<ApiKeyRedisActionEvent> logger, AuthDbContext context) 
            : base(logger, context)
        {
        }
    }
}