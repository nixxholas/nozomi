using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing.Options;
using Nozomi.Preprocessing.Singleton;
using StackExchange.Redis;

namespace Nozomi.Preprocessing.Extensions
{
    public static class StackExchangeRedisExtensions
    {
        [Obsolete]
        public static void ConfigureRedis(this IServiceCollection services, string apiKeyUserCacheConnectionString,
            string apiKeyEventCacheConnectionString)
        {
            if (!string.IsNullOrEmpty(apiKeyUserCacheConnectionString) 
                && !string.IsNullOrEmpty(apiKeyEventCacheConnectionString))
            {
                // Sample multiplexer connection string - "redis0:6380,redis1:6380,allowAdmin=true"
                // https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                services.Configure<NozomiRedisCacheOptions>(options =>
                    {
                        options.ApiKeyEventConnection = apiKeyEventCacheConnectionString;
                        options.ApiKeyUserConnection = apiKeyUserCacheConnectionString;
                    });
            }
            else
                throw new RedisConnectionException(ConnectionFailureType.Loading, "Empty Redis connection " +
                                                                                  "string.");

        }

        public static void ConfigureRedisMultiplexers(this IServiceCollection services,
            string apiKeyUserCacheConnectionString, string apiKeyEventCacheConnectionString)
        {
            if (!string.IsNullOrEmpty(apiKeyUserCacheConnectionString) 
                && !string.IsNullOrEmpty(apiKeyEventCacheConnectionString))
            {
                // Sample multiplexer connection string - "redis0:6380,redis1:6380,allowAdmin=true"
                // https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                services.Configure<NozomiRedisCacheOptions>(options =>
                {
                    options.ApiKeyEventConnection = apiKeyEventCacheConnectionString;
                    options.ApiKeyUserConnection = apiKeyUserCacheConnectionString;
                });

                services.AddScoped<ConnectionMultiplexerManager>();
            }
            else
                throw new RedisConnectionException(ConnectionFailureType.Loading, "Empty Redis connection " +
                                                                                  "string.");
        }
    }
}