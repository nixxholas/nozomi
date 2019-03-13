using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;

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
        private readonly ICurrencyEvent _currencyEvent;
        private readonly IRequestComponentEvent _requestComponentEvent;

        public ComponentAnalysisService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
            _analysedHistoricItemEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemEvent>();
            _analysedComponentService = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentService>();
            _analysedHistoricItemService = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemService>();
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
            _requestComponentEvent = _scope.ServiceProvider.GetRequiredService<IRequestComponentEvent>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var items = _analysedComponentEvent.GetAll(true, true);

                    if (Analyse(items.ToList()))
                    {
                        _logger.LogInformation($"[{ServiceName}]" +
                                               " Analysis successful");
                    }
                    else
                    {
                        _logger.LogWarning($"[{ServiceName}]" +
                                           " Something bad happened");
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

        /// <summary>
        /// Analysis Method that computes every AnalysedComponentType Enumerator
        /// </summary>
        /// <param name="components">The list of components to compute and save.</param>
        /// <returns>Success or failure of collection processing</returns>
        public bool Analyse(ICollection<AnalysedComponent> components)
        {
            foreach (var component in components)
            {
                // Always stash the value first
                if (Stash(component))
                {
                    switch (component.ComponentType)
                    {
                        // Calculate the market cap.
                        case AnalysedComponentType.MarketCap:
                            var circuSupply = _currencyEvent.GetCirculatingSupply(component);

                            if (circuSupply > 0 && components.Any(ac =>
                                    ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                    && !string.IsNullOrEmpty(ac.Value)))
                            {
                                var marketCap = circuSupply
                                                * decimal.Parse(components
                                                    .DefaultIfEmpty()
                                                    .Where(ac => ac.ComponentType.Equals(AnalysedComponentType
                                                        .CurrentAveragePrice))
                                                    .Select(ac => ac.Value)
                                                    .SingleOrDefault());

                                if (!decimal.Zero.Equals(marketCap))
                                {
                                    return _analysedComponentService.UpdateValue(component.Id, marketCap.ToString());
                                }
                            }

                            break;
                        // Calculate the current average price.
                        case AnalysedComponentType.CurrentAveragePrice:
                            // Obtain all of the req components that are related to this AC.
                            var correlatedReqComps = _requestComponentEvent.GetAllByCorrelation(component.Id);
                            
                            // Aggregate it
                            var avgPrice = correlatedReqComps
                                .Where(rc => rc.ComponentType.Equals(ComponentType.Ask) 
                                || rc.ComponentType.Equals(ComponentType.Bid))
                                .DefaultIfEmpty()
                                .Average(rc => decimal.Parse(rc.RequestComponentDatum.Value));

                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(component.Id, avgPrice.ToString());
                            }
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
                                .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(31)))
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
            else
            {
                return true; // This component does not have a historical object yet..
            }
        }
    }
}