using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Abstracts;
using Nozomi.Infra.Compute.Events.Interfaces;

namespace Nozomi.Infra.Compute.HostedServices
{
    public class ComputeHostedService : BaseComputeService<ComputeHostedService>
    {
        public ComputeHostedService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_computeServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_computeServiceName} is stopping."));

            // Initialize an event like that
            using (var scope = _scopeFactory.CreateScope())
            {
                var computeEvent = scope.ServiceProvider.GetRequiredService<IComputeEvent>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var oldCompute = computeEvent.GetMostOutdated(true);
                    if (oldCompute != null)
                        ExecuteComputation(oldCompute); // Execute!
                    else
                        _logger.LogInformation($"{_computeServiceName} ExecuteAsync: Nothing to compute.");

                    // Delay deliberately
                    await Task.Delay(500, stoppingToken);
                }
            }

            _logger.LogWarning($"{_computeServiceName} background task is stopping.");
        }
    }
}