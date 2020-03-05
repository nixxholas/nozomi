using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class ApiKeyRedisActionService : BaseService<ApiKeyRedisActionService, AuthDbContext>, 
        IApiKeyRedisActionService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public ApiKeyRedisActionService(ILogger<ApiKeyRedisActionService> logger, AuthDbContext context,
            IConnectionMultiplexer connectionMultiplexer) 
            : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public ApiKeyRedisActionService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyRedisActionService> logger, 
            AuthDbContext context, IConnectionMultiplexer connectionMultiplexer) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public void Fill(string apiKey, long fillAmount = 1, string customDescription = null)
        {
            if (!string.IsNullOrEmpty(apiKey) && fillAmount > 0)
            {
                // Let's use + in strings here to make things quicker since the above statement already has a check in
                // place.
                // https://stackoverflow.com/questions/47605/string-concatenation-concat-vs-operator
                var currentRequestEntry = apiKey + '_' + DateTimeOffset.Now.ToUnixTimeSeconds();

                if (!string.IsNullOrEmpty(customDescription)) // Add a custom description if any
                    currentRequestEntry += '_' + customDescription; 
                
                _connectionMultiplexer.GetDatabase((int) RedisDatabases.UnrecordedApiKeyEvents)
                    .SetAdd(currentRequestEntry, fillAmount);
                _logger.LogInformation($"{_serviceName} Fill: RedisKey {currentRequestEntry} added to " +
                                       $"UnrecordedApiKeyEvents cache.");
                return;
            }
            
            throw new ArgumentNullException("Invalid api key or fill amount.");
        }

        public void Clear(string apiKey)
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                _logger.LogInformation($"{_serviceName} Clear: Clearing unrecorded events for api key " +
                                       $"{apiKey}");
                var connections = _connectionMultiplexer.GetEndPoints();
                var server = _connectionMultiplexer.GetServer(connections[0]);
                var apiKeyPairs = server.Keys(pattern: $"*{apiKey}*").ToList();

                if (apiKeyPairs.Any())
                {
                    foreach (var apiKeyPair in apiKeyPairs)
                    {
                        _connectionMultiplexer.GetDatabase((int) RedisDatabases.UnrecordedApiKeyEvents)
                            .KeyDelete(apiKeyPair);
                    }

                    _logger.LogInformation($"{_serviceName} Clear: Unrecorded events cleared for key " +
                                           $"{apiKey}.");
                    return;
                }
            }
            
            throw new NullReferenceException("Invalid api key.");
        }
    }
}