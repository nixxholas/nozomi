using Nozomi.Preprocessing;

namespace Nozomi.Infra.Api.Limiter.Events.Interfaces
{
    public interface INozomiRedisEvent
    {
        bool Exists(string key, RedisDatabases redisDatabase = RedisDatabases.BlockedApiKeys);
    }
}