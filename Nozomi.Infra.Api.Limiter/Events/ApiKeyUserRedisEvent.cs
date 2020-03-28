using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Options;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class ApiKeyUserRedisEvent : BaseEvent<ApiKeyUserRedisEvent, AuthDbContext>, IApiKeyUserRedisEvent
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public ApiKeyUserRedisEvent(ILogger<ApiKeyUserRedisEvent> logger, AuthDbContext context,
            IOptions<NozomiRedisCacheOptions> options) 
            : base(logger, context)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.ApiKeyUserConnection);
        }

        public bool CanPour(string key)
        {
            if (!string.IsNullOrEmpty(key)) // Check if the key is null
            {
                // var redisValue = _connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyUser)
                //     .StringGet(key); // Obtain the value

                // If the value is null or empty, this guy is banned
                return !_connectionMultiplexer.GetDatabase().StringGet(key).IsNullOrEmpty; 
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}