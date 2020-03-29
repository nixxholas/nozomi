using System;
using System.Collections.Generic;
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
    public class NozomiRedisEvent : BaseEvent<NozomiRedisEvent>, INozomiRedisEvent
    {
        private readonly ConnectionMultiplexerManager _connectionMultiplexerManager;
        
        public NozomiRedisEvent(ILogger<NozomiRedisEvent> logger, 
            ConnectionMultiplexerManager connectionMultiplexerManager) 
            : base(logger)
        {
            _connectionMultiplexerManager = connectionMultiplexerManager;
        }

        public IEnumerable<RedisKey> AllKeys(RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            switch (redisDatabase)
            {
                case RedisDatabases.ApiKeyEvents:
                    var apiKeyEventEndPoints = _connectionMultiplexerManager.ApiKeyEventMultiplexer
                        .GetEndPoints();
                    return _connectionMultiplexerManager.ApiKeyEventMultiplexer
                        .GetServer(apiKeyEventEndPoints[0]).Keys();
                default:
                    var apiKeyUserEndPoints = _connectionMultiplexerManager.ApiKeyUserMultiplexer
                        .GetEndPoints();
                    return _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetServer(apiKeyUserEndPoints[0]).Keys();
            }
        }

        public bool Exists(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                switch (redisDatabase)
                {
                    case RedisDatabases.ApiKeyEvents:
                        return _connectionMultiplexerManager.ApiKeyEventMultiplexer.GetDatabase().KeyExists(key);
                    default:
                        return _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase().KeyExists(key);
                }
            }

            throw new NullReferenceException("Invalid key.");
        }

        public bool ContainsValue(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IDatabase database;

                switch (redisDatabase)
                {
                    case RedisDatabases.ApiKeyEvents:
                        database = _connectionMultiplexerManager.ApiKeyEventMultiplexer.GetDatabase();
                        break;
                    default:
                        database = _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase();
                        break;
                }

                if (database.KeyExists(key))
                    return !database.StringGet(key).IsNullOrEmpty;

                _logger.LogInformation($"{_eventName} ContainsValue: Key {key} does not exist.");
                return false;
            }

            throw new NullReferenceException("Invalid key.");
        }

        public RedisValue GetValue(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IDatabase database;

                switch (redisDatabase)
                {
                    case RedisDatabases.ApiKeyEvents:
                        database = _connectionMultiplexerManager.ApiKeyEventMultiplexer.GetDatabase();
                        break;
                    default:
                        database = _connectionMultiplexerManager.ApiKeyUserMultiplexer.GetDatabase();
                        break;
                }

                if (database.KeyExists(key))
                {
                    return database.StringGet(key);
                }

                _logger.LogInformation($"{_eventName} ContainsValue: Key {key} does not exist.");
                return RedisValue.Null;
            }

            throw new NullReferenceException("Invalid key.");
        }
    }
}