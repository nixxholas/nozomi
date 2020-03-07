using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Api.Limiter.HostedServices
{
    public class UnrecordedApiKeyEventHostedService : BaseHostedService<UnrecordedApiKeyEventHostedService>
    {
        public UnrecordedApiKeyEventHostedService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_hostedServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_hostedServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
            }
            
            _logger.LogCritical($"{_hostedServiceName} has broken out of its loop.");
        }
    }
}