using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Options;
using Nozomi.Preprocessing.Singleton;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Services
{
    public class NozomiRedisService : BaseService<NozomiRedisService, AuthDbContext>, INozomiRedisService
    {
        private readonly ConnectionMultiplexerManager _connectionMultiplexerManager;
        
        public NozomiRedisService(ILogger<NozomiRedisService> logger, AuthDbContext context, 
            ConnectionMultiplexerManager connectionMultiplexerManager) : base(logger, context)
        {
            _connectionMultiplexerManager = connectionMultiplexerManager;
        }

        public NozomiRedisService(IHttpContextAccessor contextAccessor, ILogger<NozomiRedisService> logger, 
            AuthDbContext context, ConnectionMultiplexerManager connectionMultiplexerManager) 
            : base(contextAccessor, logger, context)
        {
            _connectionMultiplexerManager = connectionMultiplexerManager;
        }

        public void Add(RedisDatabases databasesEnum, string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                // Dont' have to set expiry if its not going to expire
                // https://github.com/StackExchange/StackExchange.Redis/issues/1095#issuecomment-473248185
                switch (databasesEnum)
                {
                    case RedisDatabases.ApiKeyEvents:
                        _connectionMultiplexerManager.ApiKeyEventMultiplexer.GetDatabase().StringSet(key, value);
                
                        _logger.LogInformation($"{_serviceName} Add: key {key} with value {value} " +
                                               "successfully set.");
                        return;
                    default:
                        _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase().StringSet(key, value);
                
                        _logger.LogInformation($"{_serviceName} Add: key {key} with value {value} " +
                                               "successfully set.");
                        return;
                }
            }
            
            throw new NullReferenceException("Invalid key or value parameter.");
        }

        public void Remove(RedisDatabases databasesEnum, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                switch (databasesEnum)
                {
                    case RedisDatabases.ApiKeyEvents:
                        _connectionMultiplexerManager.ApiKeyEventMultiplexer.GetDatabase().KeyDelete(key);
                
                        _logger.LogInformation($"{_serviceName} Remove: key {key} successfully deleted.");
                        return;
                    default:
                        _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase().KeyDelete(key);
                
                        _logger.LogInformation($"{_serviceName} Remove: key {key} successfully deleted.");
                        return;
                }
            }
            
            throw new NullReferenceException("Invalid key.");
        }
    }
}