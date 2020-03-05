using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class BlockedApiKeyRedisService : BaseService<BlockedApiKeyRedisService, AuthDbContext>, 
    IBlockedApiKeyRedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public BlockedApiKeyRedisService(ILogger<BlockedApiKeyRedisService> logger, AuthDbContext context, 
            IConnectionMultiplexer connectionMultiplexer) : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public BlockedApiKeyRedisService(IHttpContextAccessor contextAccessor, 
            ILogger<BlockedApiKeyRedisService> logger, AuthDbContext context, 
            IConnectionMultiplexer connectionMultiplexer) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                // Dont' have to set expiry if its not going to expire
                // https://github.com/StackExchange/StackExchange.Redis/issues/1095#issuecomment-473248185
                _connectionMultiplexer.GetDatabase().StringSet(key, value);
                
                _logger.LogInformation($"{_serviceName} Add: key {key} with value {value} successfully set.");
                return;
            }
            
            throw new NullReferenceException("Invalid key or value parameter.");
        }

        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _connectionMultiplexer.GetDatabase().KeyDelete(key);
                
                _logger.LogInformation($"{_serviceName} Remove: key {key} successfully deleted.");
                return;
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}