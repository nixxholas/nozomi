using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
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
    public class AcAnalysisHostedService : BaseHostedService<AcAnalysisHostedService>,
        IAnalysisHostedService<AnalysedComponent>
    {
        private const string ServiceName = "AcAnalysisHostedService";
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly IComponentEvent _componentEvent;
        private readonly IXAnalysedComponentEvent _xAnalysedComponentEvent;
        private readonly IProcessAnalysedComponentService _processAnalysedComponentService;

        public AcAnalysisHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
            _analysedHistoricItemEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedHistoricItemEvent>();
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
            _currencyPairEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyPairEvent>();
            _componentEvent = _scope.ServiceProvider.GetRequiredService<IComponentEvent>();
            _xAnalysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IXAnalysedComponentEvent>();
            _processAnalysedComponentService = _scope.ServiceProvider.GetRequiredService<IProcessAnalysedComponentService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");

            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Dynamic
                    var currentBatch = _xAnalysedComponentEvent.GetNextWorkingSet(0, true);

                    if (!currentBatch.Any()) continue;
                    foreach (var batchItem in currentBatch)
                    {
                        if (Analyse(batchItem))
                        {
                            _logger.LogInformation($"[{ServiceName}] AnalysedComponent {batchItem.Id}: " +
                                                   $"Successfully to updated");
                        }
                        else
                        {   
                            _logger.LogCritical("[ComponentAnalysisService]: Invalid top AC.");
                        }
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
                if (entity.ModifiedAt.AddMilliseconds(entity.Delay) > DateTime.UtcNow)
                {
                    _processAnalysedComponentService.Checked(entity.Id, true);
                    _logger.LogWarning($"[{ServiceName}] Analyse: Skipping {entity.Id}, has been recently updated.");
                    return true;
                }
                
                var dataTimespan = TimeSpan.Zero;

                // Logic here once again
                switch (entity.ComponentType)
                {
                    case AnalysedComponentType.Unknown:
                        // If it winds up here, its fine
                        _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): Skipping, Unknown type.");
                        return _processAnalysedComponentService.Checked(entity.Id);
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
                                case AnalysedComponentType.HourlyMarketCap:
                                case AnalysedComponentType.DailyMarketCap:
                                    // Obtain the computed market cap.
                                    var obtainedComponent = _analysedComponentEvent
                                        .GetAllByCurrencyType((long) entity.CurrencyTypeId, true,
                                            // Make sure we obtain value relevant to a certain time frame.
                                            0, dataTimespan.Milliseconds)
                                        .SingleOrDefault(ac => ac.ComponentType.Equals(
                                            // If its the daily market cap, base if off hourly. Else base it off
                                            entity.ComponentType.Equals(AnalysedComponentType.DailyMarketCap)
                                                ?
                                                // just the market cap alone.
                                                AnalysedComponentType.HourlyMarketCap
                                                : AnalysedComponentType.MarketCap
                                        ));

                                    if (obtainedComponent != null && obtainedComponent.AnalysedHistoricItems.Count > 0)
                                    {
                                        obtainedComponent.AnalysedHistoricItems
                                            .Add(new AnalysedHistoricItem
                                            {
                                                Value = obtainedComponent.Value
                                            });

                                        return _processAnalysedComponentService.UpdateValue(entity.Id,
                                            obtainedComponent.AnalysedHistoricItems
                                                .Where(ahi => !string.IsNullOrEmpty(ahi.Value) 
                                                              && NumberHelper.IsNumericDecimal(ahi.Value))
                                                .Average(ahi => decimal.Parse(ahi.Value))
                                                .ToString(CultureInfo.InvariantCulture));
                                    }

                                    // Hitting here? nothing to process.
                                    _logger.LogInformation($"[{ServiceName}] Analyse Hourly/Daily Type Market " +
                                                           $"Cap({entity.Id}): nothing to log yet.");

                                    return _processAnalysedComponentService.Checked(entity.Id);
                                // Default market cap function
                                case AnalysedComponentType.MarketCap:
                                    // Obtain all sub components (Components in the currencies)
                                    var analysedComponents = _analysedComponentEvent.GetAllCurrencyComponentsByType(
                                            (long) entity.CurrencyTypeId)
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
                                            if (decimal.TryParse(ac.Value, out var val) && val > decimal.Zero
                                                && ac.Currency != null)
                                            {
                                                // Does this ticker exist on the list of market caps yet?
                                                if (marketCapByCurrencies.ContainsKey(ac.Currency.Abbreviation))
                                                {
                                                    // Since yes, let's work on averaging it
                                                    marketCapByCurrencies[ac.Currency.Abbreviation] =
                                                        decimal.Divide(
                                                            decimal.Add(
                                                                marketCapByCurrencies[ac.Currency.Abbreviation], val),
                                                            2);
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

                                            return _processAnalysedComponentService.UpdateValue(entity.Id,
                                                marketCap.ToString(CultureInfo.InvariantCulture));
                                        }
                                    }

                                    // Hitting here? nothing to process.
                                    _logger.LogInformation($"[{ServiceName}] Analyse Hourly/Daily Type Market " +
                                                           $"Cap({entity.Id}): nothing to log yet.");

                                    return _processAnalysedComponentService.Checked(entity.Id);
                            }
                        }
                        // Currency-based Market Cap
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // obtain all related entities first
                            var currencyAveragePrice = _analysedComponentEvent.GetAllByCurrency(
                                    (long) entity.CurrencyId,
                                    true, true)
                                .SingleOrDefault(ac =>
                                    !string.IsNullOrEmpty(ac.Value)
                                    && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)
                                    && NumberHelper.IsNumericDecimal(ac.Value));

                            if (currencyAveragePrice != null)
                            {
                                // Obtain the circulating supply
                                var circulatingSupply = _currencyEvent.GetCirculatingSupply(entity);

                                // Parsable average?
                                if (circulatingSupply > 0 
                                    && decimal.TryParse(currencyAveragePrice.Value, out var avgPrice) && avgPrice > 0)
                                {
                                    // Market Cap Formula
                                    var marketCap = decimal.Multiply(circulatingSupply, avgPrice);
                                    
                                    return _processAnalysedComponentService.UpdateValue(entity.Id, marketCap
                                        .ToString(CultureInfo.InvariantCulture));
                                }
                            }
                        }
                        // Request-based Market Cap
                        else
                        {
                            var circulatingSupply = _currencyEvent.GetCirculatingSupply(entity);
                            var analysedComponents = _analysedComponentEvent.GetAllByCorrelation(entity.Id,
                                    ac => !string.IsNullOrEmpty(ac.Value)
                                          && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice),
                                    ac => NumberHelper.IsNumericDecimal(ac.Value))
                                .ToList();

                            // Parsable average?
                            if (circulatingSupply > 0
                                // Parsable average?
                                && decimal.TryParse(analysedComponents
                                                        .Select(ac => ac.Value)
                                                        .FirstOrDefault() ?? "0", out var mCapAvgPrice)
                                && mCapAvgPrice > 0)
                            {
                                var marketCap = decimal.Multiply(circulatingSupply, mCapAvgPrice);

                                return _processAnalysedComponentService.UpdateValue(entity.Id, marketCap
                                    .ToString(CultureInfo.InvariantCulture));
                            }
                        }

                        break;
                    // TODO:
                    case AnalysedComponentType.MarketCapChange:
                    case AnalysedComponentType.MarketCapHourlyChange:
                    case AnalysedComponentType.MarketCapDailyChange:
                        // Disable
                        return _processAnalysedComponentService.Disable(entity.Id);
                    case AnalysedComponentType.CurrentAveragePrice:
                        // CurrencyType-based Live Average Price
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/CurrentAveragePrice: A CurrencyType-" +
                                                $"based component is attempting to compute its CurrentAveragePrice.");

                            // Disable
                            return _processAnalysedComponentService.Disable(entity.Id);
                        }

                        // Currency-based Live Average Price
                        // 1. Multiple Currency Pairs with Different Counter Currencies
                        // 2. Just one pair that has the generic counter currency
                        // 3. Just one pair that doesn't have the generic counter currency
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            var avgPrice = decimal.Zero; // Stored value for final average price
                            var index = 0; // Indexer for iterator
                            var components =  _currencyEvent.GetTickerPairComponents(
                                (long) entity.CurrencyId,
                                // Ensure that all components used are valid, no historical values are being tapped on.
                                true, index, false, ac => // Make sure its the generic counter currency
                                    // since we can't convert yet
                                    ac.CurrencyPair.CounterTicker
                                        .Equals(CoreConstants.GenericCounterCurrency)
                                    && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice), 
                                ac => NumberHelper.IsNumericDecimal(ac.Value));

                            // While there's something within the current page,
                            while (components.Any()) // iterate
                            {
                                if (!avgPrice.Equals(decimal.Zero))
                                {
                                    // Aggregate it
                                    avgPrice = decimal.Divide(decimal.Add(components
                                        .Average(ac => decimal.Parse(ac.Value)), avgPrice), 2);
                                }
                                else
                                {
                                    avgPrice = components.Average(ac => decimal.Parse(ac.Value));
                                }
                                
                                // Index Increment and iterate
                                components =  _currencyEvent.GetTickerPairComponents(
                                    (long) entity.CurrencyId,
                                    // Ensure that all components used are valid, no historical values are being tapped on.
                                    true, ++index, false, ac => // Make sure its the generic counter currency
                                        // since we can't convert yet
                                        ac.CurrencyPair.CounterTicker.ToUpper()
                                            .Equals(CoreConstants.GenericCounterCurrency.ToUpper())
                                        && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice), 
                                    ac => NumberHelper.IsNumericDecimal(ac.Value));
                            }

                            // Update!
                            if (avgPrice > 0)
                            {
                                return _processAnalysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }
                            else if (avgPrice.Equals(decimal.Zero))
                            {
                                // Send off if there's nothing yet
                                return _processAnalysedComponentService.Checked(entity.Id);
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
                            var avgPrice = decimal.Zero;
                            var index = 0;
                            var components = _currencyPairEvent.GetComponents(entity.Id, true,
                                    index, true, new List<ComponentType>()
                                    {
                                        ComponentType.Ask, ComponentType.Bid
                                    });

                            if (components.Any(c => NumberHelper.IsNumericDecimal(c.Value)))
                            {
                                // Aggregate it
                                avgPrice = components
                                    .Average(rc => decimal.Parse(rc.Value));

                                // Now that's its computed, check again
                                if (avgPrice > 0)
                                {
                                    return _processAnalysedComponentService.UpdateValue(entity.Id,
                                        avgPrice.ToString(CultureInfo.InvariantCulture));
                                }
                            
                                // Hitting here? Sum ting wong
                                _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                   $"average price can't be computed, component data might have been" +
                                                   $"contaminated..");
                            }
                            else
                            {
                                // Hitting here? Sum ting wong
                                _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                   $"average price can't be computed.");
                            }
                            
                            return _processAnalysedComponentService.Checked(entity.Id, true);
                        }

                        break;
                    case AnalysedComponentType.HourlyAveragePrice:
                        dataTimespan = TimeSpan.FromHours(1);

                        // CurrencyType-based Hourly Average Price
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/HourlyAveragePrice: A CurrencyType-" +
                                                $"based component is attempting to compute its HourlyAveragePrice.");

                            // Disable
                            return _processAnalysedComponentService.Disable(entity.Id);
                        }

                        // Currency-based Hourly Average Price
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            var compData = _analysedHistoricItemEvent.TraverseRelatedHistory(entity.Id,
                                AnalysedComponentType.CurrentAveragePrice);

                            // Safetynet
                            if (compData.Data == null)
                            {
                                _processAnalysedComponentService.Checked(entity.Id);
                                return true;
                            }
                            
                            // Aggregate it
                            var avgPrice = decimal.Zero;

                            for (var i = 0; i < compData.Pages; i++)
                            {
                                if (i > 0)
                                {
                                    var currData = _analysedHistoricItemEvent.TraverseRelatedHistory(entity.Id,
                                        AnalysedComponentType.CurrentAveragePrice, i);
                                    
                                    if (currData.Data.Any())
                                        avgPrice = decimal.Divide(
                                            avgPrice + currData.Data.Average(ahi => decimal.Parse(ahi.Value))
                                            , 2);
                                }
                                else // First page
                                {
                                    avgPrice = compData.Data
                                        .Average(ahi => decimal.Parse(ahi.Value));
                                }
                            }

                            // Update!
                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _processAnalysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"hourly average price can't be computed.");
                        }
                        // Request-based Hourly Average Price
                        // 1. This came from a CurrencyPair
                        // 2. This came from a non-currency request
                        else
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetRelevantComponentQueryCount(
                                entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Relational checks
                                                             && ahi.AnalysedComponent.CurrencyPairId != null
                                                             // Make sure the main currency matches this currency
                                                             && ahi.AnalysedComponent.CurrencyPair.Source
                                                                 .SourceCurrencies
                                                                 .Any(sc => sc.Currency.Abbreviation
                                                                     .Equals(ahi.AnalysedComponent.CurrencyPair
                                                                         .MainTicker))
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.CurrentAveragePrice),
                                // Time check
                                ahi => ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(dataTimespan), 
                                true);
                            var compsPages =
                                (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                    ? decimal.Divide(componentsToCompute,
                                        NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                    : 1;

                            // Aggregate it
                            var avgPrice = decimal.Zero;

                            for (var i = 0; i < compsPages; i++)
                            {
                                if (_currencyPairEvent.HasRelatedComponent(entity.Id,
                                    AnalysedComponentType.CurrentAveragePrice))
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
                                            avgPrice = decimal.Divide(decimal.Add(avgPrice,
                                                analysedComponent.AnalysedHistoricItems
                                                    .Average(ahi => decimal.Parse(ahi.Value))), 2);
                                        }
                                        else
                                        {
                                            avgPrice = analysedComponent.AnalysedHistoricItems
                                                .Average(ahi => decimal.Parse(ahi.Value));
                                        }
                                    }
                                }
                                else
                                {
                                    _logger.LogInformation($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                           $"hourly average price can't be computed because an average" +
                                                           $" price component is doesn't exist.");
                                }
                            }

                            if (!decimal.Zero.Equals(avgPrice))
                            {
                                return _processAnalysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"hourly average price can't be computed.");
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

                            // Disable
                            return _processAnalysedComponentService.Disable(entity.Id);
                        }

                        // Currency-based Live Average Price
                        else if (entity.CurrencyId != null && entity.CurrencyId > 0)
                        {
                            // How many components we got
                            var componentsToCompute = _analysedHistoricItemEvent.GetRelevantComponentQueryCount(
                                entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >=
                                                             DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.HourlyAveragePrice),
                                null, true);
                            var componentPages =
                                (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                    ? componentsToCompute / NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit
                                    : 1;

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
                                return _processAnalysedComponentService.UpdateValue(entity.Id,
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
                            var componentsToCompute = _analysedHistoricItemEvent.GetRelevantComponentQueryCount(
                                entity.Id,
                                // Active checks
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             // Time check
                                                             && ahi.HistoricDateTime >=
                                                             DateTime.UtcNow.Subtract(dataTimespan)
                                                             // Relational checks
                                                             && ahi.AnalysedComponent.CurrencyPair != null
                                                             // Make sure the main currency matches this currency
                                                             && ahi.AnalysedComponent.CurrencyPair.Source
                                                                 .SourceCurrencies
                                                                 .Any(sc => sc.Currency.Abbreviation
                                                                     .Equals(ahi.AnalysedComponent.CurrencyPair
                                                                         .MainTicker))
                                                             // Make sure we only check for the CurrentAveragePrice component
                                                             && ahi.AnalysedComponent.ComponentType
                                                                 .Equals(AnalysedComponentType.HourlyAveragePrice),
                                null, true);
                            var compsPages =
                                (componentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                    ? decimal.Divide(componentsToCompute,
                                        NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
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
                                        avgPrice = decimal.Divide(decimal.Add(avgPrice,
                                            analysedComponent.AnalysedHistoricItems
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
                                return _processAnalysedComponentService.UpdateValue(entity.Id,
                                    avgPrice.ToString(CultureInfo.InvariantCulture));
                            }

                            // Hitting here? Sum ting wong
                            _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                               $"average price can't be computed.");
                        }

                        break;
                    // TODO:
                    case AnalysedComponentType.MarketCapPctChange:
                        // Disable
                        return _processAnalysedComponentService.Disable(entity.Id);
                    // TODO:
                    case AnalysedComponentType.DailyPriceChange:
                    case AnalysedComponentType.WeeklyPriceChange:
                    case AnalysedComponentType.MonthlyPriceChange:
                        // Disable
                        return _processAnalysedComponentService.Disable(entity.Id);
                    case AnalysedComponentType.MarketCapHourlyPctChange:
                    case AnalysedComponentType.MarketCapDailyPctChange:
                    case AnalysedComponentType.HourlyPricePctChange:
                    case AnalysedComponentType.DailyPricePctChange:
                        // CurrencyType-based PricePctChange
                        if (entity.CurrencyTypeId != null && entity.CurrencyTypeId > 0)
                        {
                            // This is not good lol
                            _logger.LogCritical($"[{ServiceName} / ID: {entity.Id}] " +
                                                $"Analyse/PricePctChange: A CurrencyType-" +
                                                $"based component is attempting to compute its PricePctChange.");

                            // Disable
                            return _processAnalysedComponentService.Disable(entity.Id);
                        }

                        var pctChangeComponentType = AnalysedComponentType.Unknown;
                        switch (entity.ComponentType)
                        {
                            case AnalysedComponentType.HourlyPricePctChange:
                                dataTimespan = TimeSpan.FromHours(1);
                                pctChangeComponentType = AnalysedComponentType.CurrentAveragePrice;
                                break;
                            case AnalysedComponentType.DailyPricePctChange:
                                dataTimespan = TimeSpan.FromHours(24);
                                pctChangeComponentType = AnalysedComponentType.HourlyAveragePrice;
                                break;
                            case AnalysedComponentType.MarketCapHourlyPctChange:
                                dataTimespan = TimeSpan.FromHours(1);
                                pctChangeComponentType = AnalysedComponentType.MarketCap;
                                break;
                            case AnalysedComponentType.MarketCapDailyPctChange:
                                dataTimespan = TimeSpan.FromHours(24);
                                pctChangeComponentType = AnalysedComponentType.HourlyMarketCap;
                                break;
                        }

                        // How many components we got
                        var pctChangeComponentsToCompute = _analysedHistoricItemEvent.GetRelevantComponentQueryCount(
                            entity.Id,
                            // Active checks
                            ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                         // Make sure we only check for the correct component
                                                         && ahi.AnalysedComponent.ComponentType
                                                             .Equals(pctChangeComponentType),
                                                           // Time check
                                                           ahi => ahi.HistoricDateTime >=
                                                           DateTime.UtcNow.Subtract(dataTimespan), true);

                        if (pctChangeComponentsToCompute > 0)
                        {
                            var ppctcComponentPages =
                                (pctChangeComponentsToCompute > NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                                    ? pctChangeComponentsToCompute / NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit
                                    : 1;

                            // Currency-based PricePctChange
                            // TODO: Test
                            if (entity.CurrencyId != null && entity.CurrencyId > 0)
                            {
                                // Iterate the page
                                for (var i = 0; i < ppctcComponentPages; i++)
                                {
                                    var historicData = _analysedHistoricItemEvent.GetRelevantHistorics(entity.Id,
                                            ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                                         // Time check
                                                                         && ahi.HistoricDateTime >=
                                                                         DateTime.UtcNow.Subtract(dataTimespan)
                                                                         // Make sure we only check for the CurrentAveragePrice component
                                                                         && ahi.AnalysedComponent.ComponentType
                                                                             .Equals(pctChangeComponentType)
                                                                         && NumberHelper.IsNumericDecimal(ahi.Value), i)
                                        .ToList();

                                    // You need more than 1 to compute.
                                    // For higher data reliability, let's ensure at least 18 hours of recorded data is 
                                    // present before computing.
                                    if (historicData.Count > 20)
                                    {
                                        // Obtain the percentage diff.
                                        // NEW - OLD / OLD * 100%
                                        var compute = decimal.Multiply(
                                            decimal.Divide(
                                                decimal.Subtract(
                                                    decimal.Parse(historicData.Last().Value),
                                                    decimal.Parse(historicData.First().Value)),
                                                decimal.Parse(historicData.First().Value)),
                                            100);

                                        // Update!
                                        if (!decimal.Zero.Equals(compute))
                                        {
                                            return _processAnalysedComponentService.UpdateValue(entity.Id,
                                                compute.ToString(CultureInfo.InvariantCulture));
                                        }
                                    }
                                    
                                    return _processAnalysedComponentService.Checked(entity.Id);
                                }

                                // Hitting here? Sum ting wong
                                _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                   $"PricePctChange can't be computed.");
                                return _processAnalysedComponentService.Checked(entity.Id, true);
                            }
                            // Request-based PricePctChange
                            else
                            {
                                // Iterate the page
                                for (var i = 0; i < ppctcComponentPages; i++)
                                {
                                    var historicData = _analysedHistoricItemEvent.GetRelevantHistorics(entity.Id,
                                            ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                                         // Time check
                                                                         && ahi.HistoricDateTime >=
                                                                         DateTime.UtcNow.Subtract(dataTimespan)
                                                                         // Make sure we only check for the CurrentAveragePrice component
                                                                         && ahi.AnalysedComponent.ComponentType
                                                                             .Equals(pctChangeComponentType)
                                                                         && NumberHelper.IsNumericDecimal(ahi.Value), i)
                                        .ToList();

                                    // TODO: Confirm Working
                                    // You need more than 1 to compute.
                                    // For higher data reliability, let's ensure at least 18 hours of recorded data is 
                                    // present before computing.
                                    if (historicData.Count > 20)
                                    {
                                        // Obtain the percentage diff.
                                        // NEW - OLD / OLD * 100%
                                        var compute = decimal.Multiply(
                                            decimal.Divide(
                                                decimal.Subtract(
                                                    decimal.Parse(historicData.Last().Value),
                                                    decimal.Parse(historicData.First().Value)),
                                                decimal.Parse(historicData.First().Value)),
                                            100);

                                        // Update!
                                        if (!decimal.Zero.Equals(compute))
                                        {
                                            return _processAnalysedComponentService.UpdateValue(entity.Id,
                                                compute.ToString(CultureInfo.InvariantCulture));
                                        }
                                    }
                                    
                                    return _processAnalysedComponentService.Checked(entity.Id);
                                }

                                // Hitting here? Sum ting wong
                                _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): " +
                                                   $"PricePctChange can't be computed.");
                                return _processAnalysedComponentService.Checked(entity.Id, true);
                            }
                        }
                        
                        // Nothing to check, nothing much i guess.
                        return _processAnalysedComponentService.Checked(entity.Id);
                    // TODO:
                    case AnalysedComponentType.DailyVolume:
                        return _processAnalysedComponentService.Checked(entity.Id);
                    default:
                        // If it winds up here, it needs help lol...
                        _logger.LogWarning($"[{ServiceName}] Analyse ({entity.Id}): Analysis for this type " +
                                           "is not available yet.");

                        // Disable
                        return _processAnalysedComponentService.Disable(entity.Id);
                }

                _processAnalysedComponentService.Checked(entity.Id, true);
                _logger.LogWarning($"[{ServiceName}] Analyse: Can't seem to process {entity.Id}.");
                return true;
            }

            _logger.LogCritical($"[{ServiceName}] Analyse: Critical error here. Wow.");

            return false;
        }
    }
}