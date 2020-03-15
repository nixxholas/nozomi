using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public class ApiKeyEvent : BaseEvent<ApiKeyEvent, AuthDbContext>, IApiKeyEvent
    {
        public ApiKeyEvent(ILogger<ApiKeyEvent> logger, AuthDbContext context) : base(logger, context)
        {
        }
    }
}