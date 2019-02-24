using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.HostedServices.Analytical.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.HostedServices.Analytical
{
    public class ComponentAnalysisService : BaseHostedService<ComponentAnalysisService>, IHostedService,
        IDisposable, IComponentAnalysisService
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly IAnalysedHistoricItemService _analysedHistoricItemService;
        
        public ComponentAnalysisService(IServiceProvider serviceProvider,
        IAnalysedComponentEvent analysedComponentEvent, IAnalysedHistoricItemService analysedHistoricItemService) 
            : base(serviceProvider)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _analysedHistoricItemService = analysedHistoricItemService;
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

        public bool Analyse(AnalysedComponent component)
        {
            throw new NotImplementedException();
        }

        public bool Stash(AnalysedComponent component)
        {
            throw new NotImplementedException();
        }
    }
}