using System;
using Microsoft.Extensions.Options;
using Nozomi.Preprocessing.Options;
using StackExchange.Redis;

namespace Nozomi.Preprocessing.Singleton
{
    public class ConnectionMultiplexerManager
    {
        private static IOptions<NozomiRedisCacheOptions> _options;

        public ConnectionMultiplexerManager(IOptions<NozomiRedisCacheOptions> options)
        {
            _options = options;
        }

        private static readonly Lazy<ConnectionMultiplexer> ApiKeyUserCacheLazyConnection = 
            new Lazy<ConnectionMultiplexer>(() =>
            {
                var apiKeyUserConnectionStr = _options.Value.ApiKeyUserConnection;
                return ConnectionMultiplexer.Connect(apiKeyUserConnectionStr);
            });

        public ConnectionMultiplexer ApiKeyUserMultiplexer => ApiKeyUserCacheLazyConnection.Value;

        private static readonly Lazy<ConnectionMultiplexer> ApiKeyEventCacheLazyConnection = 
            new Lazy<ConnectionMultiplexer>(() =>
            {
                var apiKeyEventConnectionStr = _options.Value.ApiKeyEventConnection;
                return ConnectionMultiplexer.Connect(apiKeyEventConnectionStr);
            });

        public ConnectionMultiplexer ApiKeyEventMultiplexer => ApiKeyEventCacheLazyConnection.Value;
    }
}