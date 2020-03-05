using System;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class ApiKeyRedisActionEvent : BaseEvent<ApiKeyRedisActionEvent, AuthDbContext>, IApiKeyRedisActionEvent
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public ApiKeyRedisActionEvent(ILogger<ApiKeyRedisActionEvent> logger, AuthDbContext context,
            IConnectionMultiplexer connectionMultiplexer) 
            : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public bool CanPour(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var redisValue = _connectionMultiplexer.GetDatabase((int) NozomiRedisDatabase.BlockedApiKeys)
                    .StringGet(key);

                return redisValue.IsNullOrEmpty;
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}