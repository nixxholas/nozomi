using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class ApiKeyEventsService : BaseService<ApiKeyEventsService, AuthDbContext>, 
        IApiKeyEventsService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public ApiKeyEventsService(ILogger<ApiKeyEventsService> logger, AuthDbContext context,
            INozomiRedisEvent nozomiRedisEvent, IConnectionMultiplexer connectionMultiplexer) 
            : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        public ApiKeyEventsService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyEventsService> logger, 
            AuthDbContext context, INozomiRedisEvent nozomiRedisEvent, IConnectionMultiplexer connectionMultiplexer) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        public void Fill(string apiKey, long fillAmount = 1, string customDescription = null)
        {
            if (!string.IsNullOrEmpty(apiKey) && fillAmount > 0)
            {
                // Let's use + in strings here to make things quicker since the above statement already has a check in
                // place.
                // https://stackoverflow.com/questions/47605/string-concatenation-concat-vs-operator
                // var currentRequestEntry = apiKey + '_' + DateTimeOffset.Now.ToUnixTimeSeconds();

                // TODO: Support for custom descriptions
                if (!string.IsNullOrEmpty(customDescription)) // Add a custom description if any
                    customDescription += '_' + DateTime.UtcNow.ToEpochTime();
                
                if (!_nozomiRedisEvent.Exists(apiKey, (int) RedisDatabases.ApiKeyEvents)) {}
                    Create(apiKey); // Create the api key into the redis cache first

                // Always push a new item to the right
                _connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyEvents)
                    .ListRightPush(apiKey, fillAmount);
                _logger.LogInformation($"{_serviceName} Fill: API key usage {apiKey} of {fillAmount} tokens " +
                                       "added to ApiKeyEvents cache.");
                return;
            }
            
            throw new ArgumentNullException("Invalid api key or fill amount.");
        }

        [Obsolete]
        public void Clear(string apiKey)
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                _logger.LogInformation($"{_serviceName} Clear: Clearing unrecorded events for api key " +
                                       $"{apiKey}");
                var connections = _connectionMultiplexer.GetEndPoints();
                var server = _connectionMultiplexer.GetServer(connections[0]);
                // https://stackoverflow.com/questions/41247952/stackexchange-redis-delete-all-keys-that-start-with
                var apiKeyPairs = server.Keys(pattern: $"*{apiKey}*", pageSize: 1000).ToList();

                if (apiKeyPairs.Any())
                {
                    foreach (var apiKeyPair in apiKeyPairs)
                    {
                        _connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyEvents)
                            .KeyDelete(apiKeyPair);
                    }

                    _logger.LogInformation($"{_serviceName} Clear: Unrecorded events cleared for key " +
                                           $"{apiKey}.");
                    return;
                }
            }
            
            throw new NullReferenceException("Invalid api key.");
        }

        public void Create(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (!_nozomiRedisEvent.Exists(key, (int) RedisDatabases.ApiKeyEvents))
                {
                    // Since it doesn't exist yet, create it
                    _connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyEvents)
                        .StringSet(key, RedisValue.Null);
                    _logger.LogInformation($"{_serviceName} Create: Key {key} added to cache.");

                    return;
                }
                
                _logger.LogWarning($"{_serviceName} Create: Key {key} already exists.");
                throw new DuplicateNameException("Duplicate key.");
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}