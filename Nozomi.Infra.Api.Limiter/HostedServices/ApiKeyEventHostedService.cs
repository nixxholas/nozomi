using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using StackExchange.Redis;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    /// <summary>
    /// UnrecordedApiKeyEventHostedService
    ///
    /// Maintains the UnrecordedApiKeyEvents Redis Cache.
    /// 1. Pops the cached items as soon as possible
    /// 2. Processes the user claims for quota usage with the popped items
    /// </summary>
    public class ApiKeyEventHostedService : BaseHostedService<ApiKeyEventHostedService>
    {
        public ApiKeyEventHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_hostedServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Redis connect!
                    var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
                    var database = connectionMultiplexer.GetDatabase((int) RedisDatabases.ApiKeyEvents);
                }
            }
            
            _logger.LogCritical($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}