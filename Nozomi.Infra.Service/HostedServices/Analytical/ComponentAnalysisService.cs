using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.HostedServices.Analytical
{
    public class ComponentAnalysisService : BaseHostedService<ComponentAnalysisService>, IHostedService,
        IDisposable
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        
        public ComponentAnalysisService(IServiceProvider serviceProvider,
        IAnalysedComponentEvent analysedComponentEvent) : base(serviceProvider)
        {
            _analysedComponentEvent = analysedComponentEvent;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ComponentAnalysisService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("ComponentAnalysisService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var items = _analysedComponentEvent.GetAll(true, true);

                    foreach (var ac in items)
                    {
                        // analyse?
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[ComponentAnalysisService]: " + ex);
                }
                
                // No naps taken
                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogWarning("ComponentAnalysisService background task is stopping.");
        }
    }
}