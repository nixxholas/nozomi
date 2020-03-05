using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class NozomiRedisEvent : BaseEvent<NozomiRedisEvent, AuthDbContext>, INozomiRedisEvent
    {
        public NozomiRedisEvent(ILogger<NozomiRedisEvent> logger, AuthDbContext context) : base(logger, context)
        {
        }
    }
}