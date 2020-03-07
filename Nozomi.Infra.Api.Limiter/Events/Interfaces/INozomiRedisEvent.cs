using System.Collections.Generic;
using Nozomi.Preprocessing;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Events.Interfaces
{
    public interface INozomiRedisEvent
    {
        IEnumerable<RedisKey> AllKeys(RedisDatabases redisDatabase = RedisDatabases.Default);
        
        bool Exists(string key, RedisDatabases redisDatabase = RedisDatabases.Default);

        bool ContainsValue(string key, RedisDatabases redisDatabase = RedisDatabases.Default);
    }
}