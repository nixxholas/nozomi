using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Options;
using Nozomi.Preprocessing.Singleton;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class ApiKeyUserRedisEvent : BaseEvent<ApiKeyUserRedisEvent, AuthDbContext>, IApiKeyUserRedisEvent
    {
        private readonly ConnectionMultiplexerManager _connectionMultiplexerManager;
        
        public ApiKeyUserRedisEvent(ILogger<ApiKeyUserRedisEvent> logger, AuthDbContext context,
            ConnectionMultiplexerManager connectionMultiplexerManager) 
            : base(logger, context)
        {
            _connectionMultiplexerManager = connectionMultiplexerManager;
        }

        public bool CanPour(string key)
        {
            if (!string.IsNullOrEmpty(key)) // Check if the key is null
            {
                // If the value is null or empty, this guy is banned
                return !_connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase().StringGet(key).IsNullOrEmpty; 
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}