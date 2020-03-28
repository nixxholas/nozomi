using System;
using System.Collections.Generic;
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
    public class NozomiRedisEvent : BaseEvent<NozomiRedisEvent>, INozomiRedisEvent
    {
        private readonly IConnectionMultiplexer _apiKeyEventsConnectionMultiplexer;
        private readonly IConnectionMultiplexer _apiKeyUsersConnectionMultiplexer;
        
        public NozomiRedisEvent(ILogger<NozomiRedisEvent> logger, IOptions<NozomiRedisCacheOptions> options) 
            : base(logger)
        {
            _apiKeyEventsConnectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.ApiKeyEventConnection);
            _apiKeyUsersConnectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.ApiKeyUserConnection);
        }

        public IEnumerable<RedisKey> AllKeys(RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            switch (redisDatabase)
            {
                case RedisDatabases.ApiKeyEvents:
                    var apiKeyEventEndPoints = _apiKeyEventsConnectionMultiplexer.GetEndPoints();
                    return _apiKeyEventsConnectionMultiplexer.GetServer(apiKeyEventEndPoints[0]).Keys();
                default:
                    var apiKeyUserEventEndPoints = _apiKeyUsersConnectionMultiplexer.GetEndPoints();
                    return _apiKeyUsersConnectionMultiplexer.GetServer(apiKeyUserEventEndPoints[0]).Keys();
            }
        }

        public bool Exists(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                switch (redisDatabase)
                {
                    case RedisDatabases.ApiKeyEvents:
                        return _apiKeyEventsConnectionMultiplexer.GetDatabase().KeyExists(key);
                    default:
                        return _apiKeyUsersConnectionMultiplexer.GetDatabase().KeyExists(key);
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
                        database = _apiKeyEventsConnectionMultiplexer.GetDatabase();
                        break;
                    default:
                        database = _apiKeyUsersConnectionMultiplexer.GetDatabase();
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
                        database = _apiKeyEventsConnectionMultiplexer.GetDatabase();
                        break;
                    default:
                        database = _apiKeyUsersConnectionMultiplexer.GetDatabase();
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