using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.Events
{
    public class NozomiRedisEvent : BaseEvent<NozomiRedisEvent, AuthDbContext>, INozomiRedisEvent
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        
        public NozomiRedisEvent(ILogger<NozomiRedisEvent> logger, AuthDbContext context,
            IConnectionMultiplexer connectionMultiplexer) : base(logger, context)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public IEnumerable<RedisKey> AllKeys(RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            return _connectionMultiplexer.GetServer(endpoints[0]).Keys((int) redisDatabase);
        }

        public bool Exists(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var database = _connectionMultiplexer.GetDatabase((int) redisDatabase);

                return database.KeyExists(key);
            }

            throw new NullReferenceException("Invalid key.");
        }

        public bool ContainsValue(string key, RedisDatabases redisDatabase = RedisDatabases.Default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var database = _connectionMultiplexer.GetDatabase((int) redisDatabase);

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
                var database = _connectionMultiplexer.GetDatabase((int) redisDatabase);

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