using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using VaultSharp;

namespace Nozomi.Api.Limiter.Extensions
{
    public static class StartupRedis
    {
        public static void ConfigureRedis(this IServiceCollection services, string configurationString)
        {
            if (!string.IsNullOrEmpty(configurationString))
                // Sample multiplexer connection string - "redis0:6380,redis1:6380,allowAdmin=true"
                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(configurationString));
            else
                throw new RedisConnectionException(ConnectionFailureType.Loading, "");

        }
    }
}