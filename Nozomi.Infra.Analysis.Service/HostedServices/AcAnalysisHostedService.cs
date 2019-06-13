using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Helpers.Native.Numerals;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class AcAnalysisHostedService : BaseHostedService<AcAnalysisHostedService>, IAnalysisHostedService<AnalysedComponent>
    {
        private const string ServiceName = "AcAnalysisHostedService";
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly IRequestComponentEvent _requestComponentEvent;
        private readonly IXAnalysedComponentEvent _xAnalysedComponentEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        
        public AcAnalysisHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
            _analysedHistoricItemEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemEvent>();
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
            _currencyPairEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyPairEvent>();
            _requestComponentEvent = _scope.ServiceProvider.GetRequiredService<IRequestComponentEvent>();
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
                var dataTimespan = TimeSpan.Zero;
                ICollection<AnalysedComponent> analysedComponents;
                
                // Logic here once again
                switch (entity.ComponentType)
                {
                    case AnalysedComponentType.Unknown:
                        // If it winds up here, its fine
                        _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): Skipping, Unknown type.");
                        break;
                    case AnalysedComponentType.HourlyMarketCap:
                        dataTimespan = TimeSpan.FromHours(1);
                        // https://stackoverflow.com/questions/3108888/why-does-c-sharp-have-break-if-its-not-optional
                        goto case AnalysedComponentType.MarketCap;
                    case AnalysedComponentType.DailyMarketCap:
                        dataTimespan = TimeSpan.FromHours(24);
                        // https://stackoverflow.com/questions/3108888/why-does-c-sharp-have-break-if-its-not-optional
                        goto case AnalysedComponentType.MarketCap;
                    case AnalysedComponentType.MarketCap:
                        // CurrencyType-based market cap
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            switch (entity.ComponentType)
                            {
                                // TODO: TEST
                                case AnalysedComponentType.HourlyMarketCap:
                                case AnalysedComponentType.DailyMarketCap:
                                    // Obtain the computed market cap.
                                    var obtainedComponent = _analysedComponentEvent
                                        .GetAllByCurrencyType((long) entity.CurrencyTypeId, true,
                                            // Make sure we obtain value relevant to a certain time frame.
                                            0, dataTimespan.Milliseconds)
                                        .SingleOrDefault(ac => ac.ComponentType.Equals(
                                            // If its the daily market cap, base if off hourly. Else base it off
                                            entity.ComponentType.Equals(AnalysedComponentType.DailyMarketCap) ?
                                                // just the market cap alone.
                                            AnalysedComponentType.HourlyMarketCap : AnalysedComponentType.MarketCap
                                            ));
                                    
                                    if (obtainedComponent != null && obtainedComponent.AnalysedHistoricItems.Count > 0)
                                    {
                                        obtainedComponent.AnalysedHistoricItems
                                            .Add(new AnalysedHistoricItem
                                            {
                                                Value = obtainedComponent.Value
                                            });
                                        
                                        return _analysedComponentService.UpdateValue(entity.Id, 
                                            obtainedComponent.AnalysedHistoricItems
                                                .Select(ahi => decimal.Parse(ahi.Value))
                                                .ToList()
                                                .Average()
                                                .ToString(CultureInfo.InvariantCulture));
                                    }
                                    
                                    // Hitting here? nothing to process.
                                    _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                       $"nothing to log yet.");
                                    
                                    break;
                                // Default market cap function
                                case AnalysedComponentType.MarketCap:
                                    // Obtain all sub components (Components in the currencies)
                                    analysedComponents = _analysedComponentEvent.GetAllCurrencyComponentsByType(
                                            (long) entity.CurrencyTypeId, false)
                                        .Where(ac => ac.ComponentType.Equals(entity.ComponentType))
                                        .ToList();

                                    if (analysedComponents.Count > 0)
                                    {
                                        // Compute the market cap now since we can get in
                                        var marketCapByCurrencies = new Dictionary<string, decimal>();

                                        // Compute per-currency first
                                        foreach (var ac in analysedComponents)
                                        {
                                            // Value check first
                                            if (decimal.TryParse(ac.Value, out var val) && val > decimal.Zero)
                                            {
                                                // Does this ticker exist on the list of market caps yet?
                                                if (marketCapByCurrencies.ContainsKey(ac.Currency.Abbreviation))
                                                {
                                                    // Since yes, let's work on averaging it
                                                    marketCapByCurrencies[ac.Currency.Abbreviation] =
                                                        decimal.Divide(
                                                            decimal.Add(
                                                                marketCapByCurrencies[ac.Currency.Abbreviation], val), 2);
                                                }
                                                else
                                                {
                                                    // Since no, let's set it
                                                    marketCapByCurrencies.Add(ac.Currency.Abbreviation, val);
                                                }
                                            }
                                        }

                                        // Compute market cap now.
                                        if (marketCapByCurrencies.Count > 0)
                                        {
                                            var marketCap = marketCapByCurrencies.Sum(item => item.Value);

                                            return _analysedComponentService.UpdateValue(entity.Id,
                                                marketCap.ToString(CultureInfo.InvariantCulture));
                                        }
                                    }
                                    break;
                            }
                        }
                        // Currency-based Market Cap
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // obtain all related entities first
                            var currencyAveragePrice = _analysedComponentEvent.GetAllByCurrency(
                                    (long) entity.CurrencyId,
                                    true, true)
                                .SingleOrDefault(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                                                            && NumberHelper.IsNumericDecimal(ac.Value));

                            if (currencyAveragePrice != null)
                            {
                                // Obtain the circulating supply
                                var circuSupply = _currencyEvent.GetCirculatingSupply(entity);

                                // Average everything
                                var averagePrice = decimal.Parse(currencyAveragePrice.Value);

                                // Parsable average?
                                if (circuSupply > 0 && averagePrice > decimal.Zero)
                                {
                                    var marketCap = decimal.Multiply(circuSupply, averagePrice);

                                    if (!decimal.Zero.Equals(marketCap))
                                    {
                                        return _analysedComponentService.UpdateValue(entity.Id, marketCap
                                            .ToString(CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                        }
                        // Request-based Market Cap
                        // TODO: TEST
                        else
                        {
                            var circuSupply = _currencyEvent.GetCirculatingSupply(entity);
                            analysedComponents = _analysedComponentEvent.GetAllByCorrelation(entity.Id,
                                    ac => ac.ComponentType
                                              .Equals(AnalysedComponentType.CurrentAveragePrice)
                                          && NumberHelper.IsNumericDecimal(ac.Value))
                                .ToList();

                            // Parsable average?
                            if (circuSupply > 0
                                // Parsable average?
                                && decimal.TryParse(analysedComponents
                                                        .Select(ac => ac.Value)
                                                        .FirstOrDefault() ?? "0", out var mCapAvgPrice))
                            {
                                var marketCap = decimal.Multiply(circuSupply, mCapAvgPrice);

                                if (!decimal.Zero.Equals(marketCap))
                                {
                                    return _analysedComponentService.UpdateValue(entity.Id, marketCap
                                        .ToString(CultureInfo.InvariantCulture));
                                }
                            }
                        }

                        break;
                    case AnalysedComponentType.MarketCapChange:
                        goto case AnalysedComponentType.MarketCapDailyChange;
                    case AnalysedComponentType.MarketCapHourlyChange:
                        goto case AnalysedComponentType.MarketCapDailyChange;
                    case AnalysedComponentType.MarketCapDailyChange:
                        break;
                    case AnalysedComponentType.MarketCapPctChange:
                        break;
                    case AnalysedComponentType.MarketCapHourlyPctChange:
                        break;
                    case AnalysedComponentType.MarketCapDailyPctChange:
                        break;
                    case AnalysedComponentType.CurrentAveragePrice:
                        // CurrencyType-based Live Average Price
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/CurrentAveragePrice: A CurrencyType-" +
                                                $"based component is attempting to compute its CurrentAveragePrice.");
                        }
                        
                        // Currency-based Live Average Price
                        // 1. Multiple Currency Pairs with Different Counter Currencies
                        // 2. Just one pair that has the generic counter currency
                        // 3. Just one pair that doesn't have the generic counter currency
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // How many components we got
                            var componentsToCompute = _analysedComponentEvent.GetTickerPairComponentsByCurrencyCount(
                                (long) entity.CurrencyId, cp => (cp.IsEnabled && cp.DeletedAt == null
                                                                 && cp.AnalysedComponents
                                                                     .Any(ac => ac.ComponentType
                                                                         .Equals(AnalysedComponentType.CurrentAveragePrice))));
                            var componentPages = 
                                (componentsToCompute > NozomiServiceConstants.AnalysedComponentTakeoutLimit) ?
                                componentsToCompute / NozomiServiceConstants.AnalysedComponentTakeoutLimit : 1;

                            var avgPrice = decimal.Zero;
                            
                            // Iterate the page
                            for (var i = 0; i < componentPages; i++)
                            {
                                var analysedComps =
                                    _analysedComponentEvent.GetTickerPairComponentsByCurrency((long)entity.CurrencyId,
                                            true, i, true, ac => // Make sure its the generic counter currency
                                                // since we can't convert yet
                                                ac.CurrencyPair.CounterCurrencyAbbrv
                                                    .Equals(CoreConstants.GenericCounterCurrency, 
                                                        StringComparison.InvariantCultureIgnoreCase)
                                                && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                                && NumberHelper.IsNumericDecimal(ac.Value))
                                        .ToList();

                                if (analysedComps.Count > 0)
                                {
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        // Aggregate it
                                        avgPrice = decimal.Divide(decimal.Add(analysedComps
                                            .Average(ac => decimal.Parse(ac.Value)), avgPrice), 2);
                                    }
                                    else
                                    {
                                        avgPrice = analysedComps.Average(ac => decimal.Parse(ac.Value));
                                    }
                                }
                            }

                            // Update!
                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id, 
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }
                            
                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }
                        // Request-based Live Average Price
                        // 1. This came from a CurrencyPair
                        // 2. This came from a non-currency request
                        else
                        {
                            var reqCompCount = _requestComponentEvent.GetCorrelationPredicateCount(entity.Id, 
                                rc => rc.DeletedAt == null && rc.IsEnabled 
                                                           && (rc.ComponentType.Equals(ComponentType.Ask)
                                                               || rc.ComponentType.Equals(ComponentType.Bid))
                                                           && !string.IsNullOrEmpty(rc.Value)
                                                           && NumberHelper.IsNumericDecimal(rc.Value));
                            var reqCompsPages = (reqCompCount > NozomiServiceConstants.RequestComponentTakeoutLimit) ? 
                                decimal.Divide(reqCompCount, NozomiServiceConstants.RequestComponentTakeoutLimit)
                                : 1;

                            // Aggregate it
                            var avgPrice = decimal.Zero;
                            
                            for (var i = 0; i < reqCompsPages; i++)
                            {
                                // Obtain all of the req components that are related to this AC.
                                var correlatedReqComps = _requestComponentEvent.GetAllByCorrelation(entity.Id, true, 
                                        i, rc => rc.DeletedAt == null && rc.IsEnabled 
                                                                      && (rc.ComponentType.Equals(ComponentType.Ask)
                                                                          || rc.ComponentType.Equals(ComponentType.Bid))
                                                                      && !string.IsNullOrEmpty(rc.Value)
                                                                      && NumberHelper.IsNumericDecimal(rc.Value))
                                    .ToList();

                                if (correlatedReqComps.Count > 0)
                                {
                                    // Aggregate it
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        avgPrice = decimal.Divide(decimal.Add(avgPrice, correlatedReqComps
                                            .Average(rc => decimal.Parse(rc.Value))), 2);
                                    }
                                    else
                                    {
                                        avgPrice = correlatedReqComps
                                            .Average(rc => decimal.Parse(rc.Value));
                                    }
                                }
                            }

                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }

                        break;
                    case AnalysedComponentType.HourlyAveragePrice:
                        dataTimespan = TimeSpan.FromHours(1);
                        
                        // CurrencyType-based Live Average Price
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/HourlyAveragePrice: A CurrencyType-" +
                                                $"based component is attempting to compute its HourlyAveragePrice.");
                        }
                        
                        // Currency-based Live Average Price
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetQueryCount(entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.CurrentAveragePrice), 
                                true);
                            var componentPages = 
                                (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit) ?
                                componentsToCompute / NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit : 1;

                            var avgPrice = decimal.Zero;
                            
                            // Iterate the page
                            for (var i = 0; i < componentPages; i++)
                            {
                                var historicData = _analysedHistoricItemEvent.GetAll(entity.Id, dataTimespan, i)
                                    .Where(ahi => NumberHelper.IsNumericDecimal(ahi.Value))
                                    .ToList();

                                if (historicData.Count > 0)
                                {
                                    // If its not zero, aggregate it.
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        // Aggregate it
                                        avgPrice = decimal.Divide(decimal.Add(historicData
                                            .Average(ahi => decimal.Parse(ahi.Value)), avgPrice), 2);
                                    }
                                    else
                                    {
                                        avgPrice = historicData.Average(ahi => decimal.Parse(ahi.Value));
                                    }
                                }
                            }

                            // Update!
                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id, 
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }
                            
                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }
                        // Request-based Live Average Price
                        // 1. This came from a CurrencyPair
                        // 2. This came from a non-currency request
                        else
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetQueryCount(entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Relational checks
                                                             && ahi.AnalysedComponent.CurrencyPair != null
                                                             // Make sure the main currency matches this currency
                                                             && ahi.AnalysedComponent.CurrencyPair.Source
                                                                 .SourceCurrencies
                                                                 .Any(sc => sc.Currency.Abbreviation
                                                                     .Equals(ahi.AnalysedComponent.CurrencyPair.MainCurrencyAbbrv))
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.CurrentAveragePrice), 
                                true);
                            var compsPages = (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit) ? 
                                decimal.Divide(componentsToCompute, NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                : 1;

                            // Aggregate it
                            var avgPrice = decimal.Zero;
                            
                            for (var i = 0; i < compsPages; i++)
                            {
                                // Obtain all the historic items related to this AC.
                                var analysedComponent = _currencyPairEvent.GetRelatedAnalysedComponent(entity.Id,
                                    AnalysedComponentType.CurrentAveragePrice, true);

                                if (analysedComponent?.AnalysedHistoricItems != null 
                                    && analysedComponent.AnalysedHistoricItems.Count > 0)
                                {
                                    // Aggregate it
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        avgPrice = decimal.Divide(decimal.Add(avgPrice, analysedComponent.AnalysedHistoricItems
                                            .Average(ahi => decimal.Parse(ahi.Value))), 2);
                                    }
                                    else
                                    {
                                        avgPrice = analysedComponent.AnalysedHistoricItems
                                            .Average(ahi => decimal.Parse(ahi.Value));
                                    }
                                }
                            }

                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }

                        break;
                    case AnalysedComponentType.DailyAveragePrice:
                        dataTimespan = TimeSpan.FromHours(24);
                        
                        // CurrencyType-based Live Average Price
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/DailyAveragePrice: A CurrencyType-" +
                                                $"based component is attempting to compute its DailyAveragePrice.");
                        }
                        
                        // Currency-based Live Average Price
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetQueryCount(entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.HourlyAveragePrice), 
                                true);
                            var componentPages = 
                                (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit) ?
                                componentsToCompute / NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit : 1;

                            var avgPrice = decimal.Zero;
                            
                            // Iterate the page
                            for (var i = 0; i < componentPages; i++)
                            {
                                var historicData = _analysedHistoricItemEvent.GetAll(entity.Id, dataTimespan, i)
                                    .Where(ahi => NumberHelper.IsNumericDecimal(ahi.Value))
                                    .ToList();

                                if (historicData.Count > 0)
                                {
                                    // If its not zero, aggregate it.
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        // Aggregate it
                                        avgPrice = decimal.Divide(decimal.Add(historicData
                                            .Average(ahi => decimal.Parse(ahi.Value)), avgPrice), 2);
                                    }
                                    else
                                    {
                                        avgPrice = historicData.Average(ahi => decimal.Parse(ahi.Value));
                                    }
                                }
                            }

                            // Update!
                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id, 
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }
                            
                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }
                        // Request-based Live Average Price
                        // 1. This came from a CurrencyPair
                        // 2. This came from a non-currency request
                        else
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetQueryCount(entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Relational checks
                                                             && ahi.AnalysedComponent.CurrencyPair != null
                                                             // Make sure the main currency matches this currency
                                                             && ahi.AnalysedComponent.CurrencyPair.Source
                                                                 .SourceCurrencies
                                                                 .Any(sc => sc.Currency.Abbreviation
                                                                     .Equals(ahi.AnalysedComponent.CurrencyPair.MainCurrencyAbbrv))
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.HourlyAveragePrice), 
                                true);
                            var compsPages = (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit) ? 
                                decimal.Divide(componentsToCompute, NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                : 1;

                            // Aggregate it
                            var avgPrice = decimal.Zero;
                            
                            for (var i = 0; i < compsPages; i++)
                            {
                                // Obtain all the historic items related to this AC.
                                var analysedComponent = _currencyPairEvent.GetRelatedAnalysedComponent(entity.Id,
                                    AnalysedComponentType.HourlyAveragePrice, true);

                                if (analysedComponent?.AnalysedHistoricItems != null 
                                    && analysedComponent.AnalysedHistoricItems.Count > 0)
                                {
                                    // Aggregate it
                                    if (!avgPrice.Equals(decimal.Zero))
                                    {
                                        avgPrice = decimal.Divide(decimal.Add(avgPrice, analysedComponent.AnalysedHistoricItems
                                            .Average(ahi => decimal.Parse(ahi.Value))), 2);
                                    }
                                    else
                                    {
                                        avgPrice = analysedComponent.AnalysedHistoricItems
                                            .Average(ahi => decimal.Parse(ahi.Value));
                                    }
                                }
                            }

                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _analysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }

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