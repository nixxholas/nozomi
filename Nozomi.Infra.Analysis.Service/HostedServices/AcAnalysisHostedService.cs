using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class AcAnalysisHostedService : BaseHostedService<AcAnalysisHostedService>, IAnalysisHostedService<AnalysedComponent>
    {
        private const string ServiceName = "AcAnalysisHostedService";
        private readonly IXAnalysedComponentEvent _xAnalysedComponentEvent;
        
        public AcAnalysisHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _xAnalysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IXAnalysedComponentEvent>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var top = _xAnalysedComponentEvent.Top();

                    if (Analyse(top))
                    {
                        _logger.LogInformation($"[{ServiceName}] AnalysedComponent {top.Id}: Successfully to updated");
                    }
                    else
                    {
                        _logger.LogWarning($"[{ServiceName}] AnalysedComponent {top.Id}: Failed to update");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("[ComponentAnalysisService]: " + ex);
                }

                // No naps taken
                await Task.Delay(100, stoppingToken);
            }

            _logger.LogWarning("ComponentAnalysisService background task is stopping.");
        }

        public bool Analyse(AnalysedComponent entity)
        {
            throw new NotImplementedException();
        }
    }
}