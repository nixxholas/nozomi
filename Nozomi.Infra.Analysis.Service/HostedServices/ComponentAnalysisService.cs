using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Hubs;

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
        
        private readonly IHubContext<NozomiStreamHub, INozomiStreamClient> _nozomiStreamHub;

        public ComponentAnalysisService(IServiceProvider serviceProvider,
            IHubContext<NozomiStreamHub, INozomiStreamClient> nozomiStreamHub)
            : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
            _analysedHistoricItemEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemEvent>();
            _analysedComponentService = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentService>();
            _analysedHistoricItemService = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemService>();
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
            _requestComponentEvent = _scope.ServiceProvider.GetRequiredService<IRequestComponentEvent>();

            _nozomiStreamHub = nozomiStreamHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var items = _analysedComponentEvent.GetAll(true)
                        .OrderBy(ac => ac.Id);

                    if (Analyse(items.ToList()))
                    {
                        _logger.LogInformation($"[{ServiceName}]" +
                                               " Analysis successful");
                
                        // Push the updated currency data
                        await _nozomiStreamHub.Clients.Group(NozomiSocketGroup.Currencies.GetDescription())
                            .Currencies(_currencyEvent.GetAllDetailed());
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
                await Task.Delay(100, stoppingToken);
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
            if (components == null || components.Count <= 0) return false;
            
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
                            var analysedComponents = _analysedComponentEvent.GetAllByCorrelation(component.Id);

                            #if DEBUG
                            // Parsable average?
                            var averagePrice = decimal.Parse(analysedComponents
                                .Where(ac => ac.ComponentType.Equals(AnalysedComponentType
                                                 .CurrentAveragePrice))
                                .Select(ac => ac.Value)
                                .FirstOrDefault() ?? "0");
                            #endif

                            // Parsable average?
                            if (circuSupply > 0
                                // Parsable average?
                                && decimal.TryParse(analysedComponents
                                                        .Where(ac =>
                                                            ac.ComponentType.Equals(AnalysedComponentType
                                                                .CurrentAveragePrice))
                                                        .Select(ac => ac.Value)
                                                        .FirstOrDefault() ?? "0", out var mCap_avgPrice))
                            {
                                var marketCap = circuSupply
                                                * mCap_avgPrice;

                                if (!decimal.Zero.Equals(marketCap))
                                {
                                    if (_analysedComponentService.UpdateValue(component.Id, marketCap.ToString()))
                                    {
                                        // Updated successfully
                                    }
                                }
                            }

                            break;
                        // Calculate the current average price.
                        case AnalysedComponentType.CurrentAveragePrice:
                            // Which case? Allow currency to precede first.
                            if (component.CurrencyId != null && component.CurrencyId > 0)
                            {
                                // Obtain all of the req components related to this currency where it is the base.
                                var currencyReqComps =
                                    _requestComponentEvent.GetAllByCurrency((long) component.CurrencyId);

                                // Safetynet
                                if (currencyReqComps != null && currencyReqComps.Count > 0)
                                {
                                    // Filter
                                    currencyReqComps = currencyReqComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .ToList();

                                    // Convert whatever is needed
                                    _requestComponentEvent.ConvertToGenericCurrency(currencyReqComps);

                                    // Now we can aggregate this
                                    var currAvgPrice = currencyReqComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .Average(rc => decimal.Parse(string.IsNullOrEmpty(rc.Value)
                                            ? "0" : rc.Value));

                                    if (!(currAvgPrice <= decimal.Zero))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id,
                                            currAvgPrice.ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Obtain all of the req components that are related to this AC.
                                var correlatedReqComps = _requestComponentEvent.GetAllByCorrelation(component.Id);

                                if (correlatedReqComps != null && correlatedReqComps.Count > 0)
                                {
#if DEBUG
                                    var filteredCorrelatedReqComps = correlatedReqComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .ToList();
#endif

                                    // Aggregate it
                                    var avgPrice = correlatedReqComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .Average(rc => decimal.Parse(string.IsNullOrEmpty(rc.Value)
                                        ? "0" : rc.Value));

                                    if (!decimal.Zero.Equals(avgPrice))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id, avgPrice.ToString()))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }

                            break;
                        // Calculate the daily price change for this request
                        case AnalysedComponentType.DailyPriceChange:
                            // If its a currency-based AnalaysedComponent, we have to aggregate an
                            if (component.CurrencyId != null && component.CurrencyId > 0)
                            {
                                // Obtain all Average price ACs that relate to this currency
                                var currencyAnalysedComps =
                                    _analysedComponentEvent.GetAllByCurrency((long) component.CurrencyId);

                                // Safetynet
                                if (currencyAnalysedComps != null && currencyAnalysedComps.Count > 0)
                                {
                                    // Filter
                                    currencyAnalysedComps = currencyAnalysedComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .ToList();

                                    // Convert whatever is needed
                                    _analysedComponentEvent.ConvertToGenericCurrency(currencyAnalysedComps);

                                    // Now we can aggregate this
                                    var currAvgPrice = currencyAnalysedComps
                                        .DefaultIfEmpty()
                                        .Average(rc => rc.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromHours(24)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!(currAvgPrice <= decimal.Zero))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id,
                                            currAvgPrice.ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Since it's not currency-based, its currencypair-based.

                                // Obtain all of the analysed components that are related to this AC.
                                var correlatedAnaComps = _analysedComponentEvent.GetAllByCorrelation(component.Id);

                                if (correlatedAnaComps != null)
                                {
                                    // Aggregate it
                                    var avgPrice = correlatedAnaComps
                                        .Where(ac => ac.ComponentType.Equals(ComponentType.Ask)
                                                     || ac.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .Average(ac => ac.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromHours(24)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!decimal.Zero.Equals(avgPrice))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id, avgPrice
                                            .ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }

                            var dailyCompute = component.Request.RequestComponents
                                .SelectMany(rcd => rcd.RcdHistoricItems)
                                .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                .Select(rcdhi => rcdhi.Value)
                                .DefaultIfEmpty()
                                .Average(val => decimal.Parse(val));

                            if (!decimal.Zero.Equals(dailyCompute))
                            {
                                // Update
                                if (_analysedComponentService.UpdateValue(component.Id, dailyCompute.ToString()))
                                {
                                    // Updated successfully
                                }
                            }

                            break;
                        case AnalysedComponentType.WeeklyPriceChange:
                            // If its a currency-based AnalaysedComponent, we have to aggregate an
                            if (component.CurrencyId != null && component.CurrencyId > 0)
                            {
                                // Obtain all Average price ACs that relate to this currency
                                var currencyAnalysedComps =
                                    _analysedComponentEvent.GetAllByCurrency((long) component.CurrencyId);

                                // Safetynet
                                if (currencyAnalysedComps != null && currencyAnalysedComps.Count > 0)
                                {
                                    // Filter
                                    currencyAnalysedComps = currencyAnalysedComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .ToList();

                                    // Convert whatever is needed
                                    _analysedComponentEvent.ConvertToGenericCurrency(currencyAnalysedComps);

                                    // Now we can aggregate this
                                    var currAvgPrice = currencyAnalysedComps
                                        .DefaultIfEmpty()
                                        .Average(rc => rc.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!(currAvgPrice <= decimal.Zero))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id,
                                            currAvgPrice.ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Since it's not currency-based, its currencypair-based.

                                // Obtain all of the analysed components that are related to this AC.
                                var correlatedAnaComps = _analysedComponentEvent.GetAllByCorrelation(component.Id);

                                if (correlatedAnaComps != null)
                                {
                                    // Aggregate it
                                    var avgPrice = correlatedAnaComps
                                        .Where(ac => ac.ComponentType.Equals(ComponentType.Ask)
                                                     || ac.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .Average(ac => ac.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!decimal.Zero.Equals(avgPrice))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id, avgPrice
                                            .ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }

                            var weeklyCompute = component.Request.RequestComponents
                                .SelectMany(rcd => rcd.RcdHistoricItems)
                                .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
                                .Select(rcdhi => rcdhi.Value)
                                .DefaultIfEmpty()
                                .Average(val => decimal.Parse(val));

                            if (!decimal.Zero.Equals(weeklyCompute))
                            {
                                // Update
                                if (_analysedComponentService.UpdateValue(component.Id, weeklyCompute.ToString()))
                                {
                                    // Updated successfully
                                }
                            }

                            break;
                        case AnalysedComponentType.MonthlyPriceChange:
                            // If its a currency-based AnalaysedComponent, we have to aggregate an
                            if (component.CurrencyId != null && component.CurrencyId > 0)
                            {
                                // Obtain all Average price ACs that relate to this currency
                                var currencyAnalysedComps =
                                    _analysedComponentEvent.GetAllByCurrency((long) component.CurrencyId);

                                // Safetynet
                                if (currencyAnalysedComps != null && currencyAnalysedComps.Count > 0)
                                {
                                    // Filter
                                    currencyAnalysedComps = currencyAnalysedComps
                                        .Where(rc => rc.ComponentType.Equals(ComponentType.Ask)
                                                     || rc.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .ToList();

                                    // Convert whatever is needed
                                    _analysedComponentEvent.ConvertToGenericCurrency(currencyAnalysedComps);

                                    // Now we can aggregate this
                                    var currAvgPrice = currencyAnalysedComps
                                        .DefaultIfEmpty()
                                        .Average(rc => rc.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!(currAvgPrice <= decimal.Zero))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id,
                                            currAvgPrice.ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Since it's not currency-based, its currencypair-based.

                                // Obtain all of the analysed components that are related to this AC.
                                var correlatedAnaComps = _analysedComponentEvent.GetAllByCorrelation(component.Id);

                                if (correlatedAnaComps != null)
                                {
                                    // Aggregate it
                                    var avgPrice = correlatedAnaComps
                                        .Where(ac => ac.ComponentType.Equals(ComponentType.Ask)
                                                     || ac.ComponentType.Equals(ComponentType.Bid))
                                        .DefaultIfEmpty()
                                        .Average(ac => ac.AnalysedHistoricItems
                                            .Where(ahi => ahi.CreatedAt >
                                                          DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)))
                                            .DefaultIfEmpty()
                                            .Average(ahi => decimal.Parse(ahi.Value)));

                                    if (!decimal.Zero.Equals(avgPrice))
                                    {
                                        if (_analysedComponentService.UpdateValue(component.Id, avgPrice
                                            .ToString(CultureInfo.InvariantCulture)))
                                        {
                                            // Updated successfully
                                        }
                                    }
                                }
                            }

                            var monthlyCompute = component.Request.RequestComponents
                                .SelectMany(rcd => rcd.RcdHistoricItems)
                                .Where(rcdhi => rcdhi.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)))
                                .Select(rcdhi => rcdhi.Value)
                                .DefaultIfEmpty()
                                .Average(val => decimal.Parse(val));

                            if (!decimal.Zero.Equals(monthlyCompute))
                            {
                                // Update
                                if (_analysedComponentService.UpdateValue(component.Id, monthlyCompute.ToString()))
                                {
                                    // Updated successfully
                                }
                            }

                            break;
                        // Calculate the daily price percentage change.
                        // TODO: % increase = Increase ÷ Original Number × 100.
                        case AnalysedComponentType.DailyPricePctChange:
                            // If its a currency-based AnalaysedComponent, we have to aggregate an
                            if (component.CurrencyId != null && component.CurrencyId > 0)
                            {
                                // Obtain all Average price ACs that relate to this currency
                                var currencyAnalysedComps =
                                    _analysedComponentEvent.GetAllByCurrency((long) component.CurrencyId, true);

                                // Safetynet
                                if (currencyAnalysedComps != null && currencyAnalysedComps.Count > 0)
                                {
                                    // Filter
                                    var historicItems = currencyAnalysedComps
                                        .Where(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                        && ac.AnalysedHistoricItems.Count > 0)
                                        .SelectMany(ac => ac.AnalysedHistoricItems)
                                        .Where(ahi => ahi.CreatedAt > DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                        // Make sure the latest is at the top, oldest at the bottom
                                        .OrderByDescending(ahi => ahi.CreatedAt)
                                        .ToList();

                                    if (historicItems.Count > 0)
                                    {
                                        var latestValue = decimal.Parse(historicItems.First().Value
                                                                        ?? "0");
                                        var oldestValue = decimal.Parse(historicItems.Last().Value ?? "0");
                                    
                                        // Calculate the increase
                                        var increase = (latestValue - oldestValue) / oldestValue * 100;
                                    
                                        // Now we can aggregate this
                                        if (increase != decimal.Zero)
                                        {
                                            if (_analysedComponentService.UpdateValue(component.Id,
                                                increase.ToString(CultureInfo.InvariantCulture)))
                                            {
                                                // Updated successfully
                                            }
                                        }
                                    }
                                    
                                    _logger.LogWarning($"{ServiceName}: Analyse({component.Id}): " +
                                                       "DailyPricePctChange: no historical data yet.");
                                }
                            }
                            else
                            {
                                // Since it's not currency-based, its currencypair-based.

                                // Obtain all of the analysed components that are related to this AC.
                                var correlatedAnaComps = _analysedComponentEvent.GetAllByCorrelation(component.Id, true);

                                if (correlatedAnaComps != null)
                                {
                                    // Filter
                                    var historicItems = correlatedAnaComps
                                        .Where(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                        && ac.AnalysedHistoricItems.Count > 0)
                                        .SelectMany(ac => ac.AnalysedHistoricItems)
                                        .Where(ahi => ahi.CreatedAt > DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))
                                        // Make sure the latest is at the top, oldest at the bottom
                                        .OrderByDescending(ahi => ahi.CreatedAt)
                                        .ToList();

                                    if (historicItems.Count > 0)
                                    {
                                        var latestValue = decimal.Parse(historicItems.First().Value
                                                                        ?? "0");
                                        var oldestValue = decimal.Parse(historicItems.Last().Value ?? "0");
                                    
                                        // Calculate the increase
                                        var increase = (latestValue - oldestValue) / oldestValue * 100;
                                    
                                        // Now we can aggregate this
                                        if (increase != decimal.Zero)
                                        {
                                            if (_analysedComponentService.UpdateValue(component.Id,
                                                increase.ToString(CultureInfo.InvariantCulture)))
                                            {
                                                // Updated successfully
                                            }
                                        }
                                    }
                                    
                                    _logger.LogWarning($"{ServiceName}: Analyse({component.Id}): " +
                                                       "DailyPricePctChange: no historical data yet.");
                                }
                            }
                            break;
                        // Calculate the daily volume.
                        case AnalysedComponentType.DailyVolume:
                            break;
                        default:
                            return false;
                    }
                }
            }

            return true;
        }

        public bool Stash(AnalysedComponent component)
        {
            // Obtain the latest historical value for cross checking
            var lastHistorical = _analysedHistoricItemEvent.Latest(component.Id);

            if (lastHistorical != null)
            {
                // Is the value the same?
                if (lastHistorical.Value.Equals(component.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Don't have to save it
                    return true; // Stashed
                }
                
                // Precise check
                if (decimal.TryParse(lastHistorical.Value, out var lastVal) 
                && decimal.TryParse(component.Value, out var valToStash)
                && !lastVal.Equals(valToStash))
                {

                    // Save it
                    var res = _analysedHistoricItemService.Create(new AnalysedHistoricItem
                    {
                        AnalysedComponentId = component.Id,
                        HistoricDateTime = component.CreatedAt,
                        Value = component.Value
                    });

                    return res > 0;
                }
                else
                {
                    // Illegal value, duped
                    return true;
                }
            }
            else
            {
                // This component does not have a historical object yet..
                
                // Failsafe
                if (string.IsNullOrEmpty(component.Value)) return true; // Job done, don't save since its null. 
                
                // Save it
                var res = _analysedHistoricItemService.Create(new AnalysedHistoricItem
                {
                    AnalysedComponentId = component.Id,
                    HistoricDateTime = component.CreatedAt,
                    Value = component.Value
                });
                
                return res > 0;
            }
        }
    }
}