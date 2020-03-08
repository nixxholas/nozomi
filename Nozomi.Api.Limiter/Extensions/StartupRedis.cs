using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Nozomi.Api.Limiter.Extensions
{
    public static class StartupRedis
    {
        public static void ConfigureRedis(this IServiceCollection services, string configurationString)
        {
            if (!string.IsNullOrEmpty(configurationString))
                // Sample multiplexer connection string - "redis0:6380,redis1:6380,allowAdmin=true"
                // https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(configurationString));
            else
                throw new RedisConnectionException(ConnectionFailureType.Loading, "Empty Redis connection " +
                                                                                  "string.");

        }
    }
}