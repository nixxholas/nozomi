using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class CurrencyAssetSyncingService : BaseHostedService<CurrencyAssetSyncingService>, IHostedService,
        IDisposable, ICurrencyAssetSyncingService
    {
        private const string ServiceName = "CurrencyAssetSyncingService";
        private IAnalysedComponentEvent _analysedComponentEvent;
        
        public CurrencyAssetSyncingService(IServiceProvider serviceProvider,
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
//                        if (Analyse(ac))
//                        {
//                            _logger.LogInformation($"[{ServiceName}] Component {ac.Id}:" +
//                                                   " Analysis successful");
//                        }
//                        else
//                        {
//                            _logger.LogWarning($"[{ServiceName}] Component {ac.Id}:" +
//                                               " Something bad happened");
//                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"[{ServiceName}]: " + ex);
                }

                // No naps taken
                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogWarning($"{ServiceName} background task is stopping.");
        }
    }
}