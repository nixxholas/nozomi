using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class AcAnalysisHostedService : BaseHostedService<AcAnalysisHostedService>, IAnalysisHostedService<AnalysedComponent>
    {
        private const string ServiceName = "AcAnalysisHostedService";
        private readonly IXAnalysedComponentEvent _xAnalysedComponentEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        
        public AcAnalysisHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _xAnalysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IXAnalysedComponentEvent>();
            _analysedComponentService = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentService>();
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
            }

            _logger.LogWarning("ComponentAnalysisService background task is stopping.");
        }

        public bool Analyse(AnalysedComponent entity)
        {
            if (entity != null)
            {
                // Logic here once again
                switch (entity.ComponentType)
                {
                    case AnalysedComponentType.Unknown:
                        // If it winds up here, its fine
                        _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): Skipping, Unknown type.");
                        break;
                    case AnalysedComponentType.MarketCap:
                        break;
                    case AnalysedComponentType.HourlyMarketCap:
                        break;
                    case AnalysedComponentType.DailyMarketCap:
                        break;
                    case AnalysedComponentType.MarketCapChange:
                        break;
                    case AnalysedComponentType.MarketCapHourlyChange:
                        break;
                    case AnalysedComponentType.MarketCapDailyChange:
                        break;
                    case AnalysedComponentType.MarketCapPctChange:
                        break;
                    case AnalysedComponentType.MarketCapHourlyPctChange:
                        break;
                    case AnalysedComponentType.MarketCapDailyPctChange:
                        break;
                    case AnalysedComponentType.CurrentAveragePrice:
                        break;
                    case AnalysedComponentType.HourlyAveragePrice:
                        break;
                    case AnalysedComponentType.DailyAveragePrice:
                        break;
                    case AnalysedComponentType.DailyPriceChange:
                        break;
                    case AnalysedComponentType.WeeklyPriceChange:
                        break;
                    case AnalysedComponentType.MonthlyPriceChange:
                        break;
                    case AnalysedComponentType.DailyPricePctChange:
                        break;
                    case AnalysedComponentType.HourlyPricePctChange:
                        break;
                    case AnalysedComponentType.DailyVolume:
                        break;
                    default:
                        // If it winds up here, it needs help lol...
                        _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): Unable to execute analysis.");
                        break;
                }
                
                _analysedComponentService.Checked(entity.Id);
            }

            _logger.LogCritical($"[{ServiceName}] Analyse: Critical error here. Wow.");
            
            return false;
        }
    }
}