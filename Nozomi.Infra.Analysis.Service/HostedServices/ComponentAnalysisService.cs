using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class ComponentAnalysisService : BaseHostedService<ComponentAnalysisService>, IHostedService,
        IDisposable, IComponentAnalysisService
    {
        private const string ServiceName = "ComponentAnalysisService";
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        private readonly IAnalysedHistoricItemService _analysedHistoricItemService;

        public ComponentAnalysisService(IServiceProvider serviceProvider,
            IAnalysedComponentEvent analysedComponentEvent, IAnalysedHistoricItemEvent analysedHistoricItemEvent,
            IAnalysedComponentService analysedComponentService,
            IAnalysedHistoricItemService analysedHistoricItemService)
            : base(serviceProvider)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _analysedComponentService = analysedComponentService;
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
                        if (Analyse(ac))
                        {
                            _logger.LogInformation($"[{ServiceName}] Component {ac.Id}:" +
                                                   " Analysis successful");
                        }
                        else
                        {
                            _logger.LogWarning($"[{ServiceName}] Component {ac.Id}:" +
                                               " Something bad happened");
                        }
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
            // Always stash the value first
            if (Stash(component))
            {
                switch (component.ComponentType)
                {
                    // Calculate the market cap.
                    case AnalysedComponentType.MarketCap:
                        break;
                    // Calculate the current average price.
                    case AnalysedComponentType.CurrentAveragePrice:
                        break;
                    // Calculate the daily price change for this request
                    case AnalysedComponentType.DailyPriceChange:
                        var dailyCompute = component.Request.RequestComponents
                            .Select(rc => rc.RequestComponentDatum)
                            .SelectMany(rcd => rcd.RcdHistoricItems)
                            .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                            .Select(rcdhi => rcdhi.Value)
                            .DefaultIfEmpty()
                            .Average(val => decimal.Parse(val));

                        if (!decimal.Zero.Equals(dailyCompute))
                        {
                            // Update
                            return _analysedComponentService.UpdateValue(component.Id, dailyCompute.ToString());
                        }

                        break;
                    case AnalysedComponentType.WeeklyPriceChange:
                        var weeklyCompute = component.Request.RequestComponents
                            .Select(rc => rc.RequestComponentDatum)
                            .SelectMany(rcd => rcd.RcdHistoricItems)
                            .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                            .Select(rcdhi => rcdhi.Value)
                            .DefaultIfEmpty()
                            .Average(val => decimal.Parse(val));

                        if (!decimal.Zero.Equals(weeklyCompute))
                        {
                            // Update
                            return _analysedComponentService.UpdateValue(component.Id, weeklyCompute.ToString());
                        }

                        break;
                    case AnalysedComponentType.MonthlyPriceChange:
                        var monthlyCompute = component.Request.RequestComponents
                            .Select(rc => rc.RequestComponentDatum)
                            .SelectMany(rcd => rcd.RcdHistoricItems)
                            .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                            .Select(rcdhi => rcdhi.Value)
                            .DefaultIfEmpty()
                            .Average(val => decimal.Parse(val));

                        if (!decimal.Zero.Equals(monthlyCompute))
                        {
                            // Update
                            return _analysedComponentService.UpdateValue(component.Id, monthlyCompute.ToString());
                        }

                        break;
                    // Calculate the daily price percentage chaneg.
                    case AnalysedComponentType.DailyPricePctChange:
                        break;
                    // Calculate the daily volume.
                    case AnalysedComponentType.DailyVolume:
                        break;
                    default:
                        return false;
                }
            }

            return false; // Failed miserably while stashing
        }

        public bool Stash(AnalysedComponent component)
        {
            // Obtain the latest historical value for cross checking
            var lastHistorical = _analysedHistoricItemEvent.Latest(component.Id);

            if (lastHistorical != null)
            {
                // Is the value the same?
                if (lastHistorical.Value.Equals(component.Value))
                {
                    // Don't have to save it
                    return true; // Stashed
                }

                // Save it
                var res = _analysedHistoricItemService.Create(new AnalysedHistoricItem
                {
                    AnalysedComponentId = component.Id,
                    Value = component.Value
                });

                return res > 0;
            }

            // Always fail if defaulted
            return false;
        }
    }
}