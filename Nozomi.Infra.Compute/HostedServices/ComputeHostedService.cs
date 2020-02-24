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
        public ComputeHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{_computeServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{_computeServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                // Initialize an event like that
                var computeEvent = _scope.ServiceProvider.GetRequiredService<IComputeEvent>();
                
                // Execute!
                ExecuteComputation(computeEvent.GetMostOutdated(true));

                // Delay deliberately
                await Task.Delay(1, stoppingToken);
            }

            _logger.LogWarning($"{_computeServiceName} background task is stopping.");
        }
    }
}