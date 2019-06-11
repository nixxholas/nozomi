using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Native.Numerals;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Infra.Analysis.Service.HostedServices.Interfaces;
using Nozomi.Infra.Analysis.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.Analysis.Service.HostedServices
{
    public class AcAnalysisHostedService : BaseHostedService<AcAnalysisHostedService>, IAnalysisHostedService<AnalysedComponent>
    {
        private const string ServiceName = "AcAnalysisHostedService";
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly IXAnalysedComponentEvent _xAnalysedComponentEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        
        public AcAnalysisHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _analysedComponentEvent = _scope.ServiceProvider.GetRequiredService<IAnalysedComponentEvent>();
            _currencyEvent = _scope.ServiceProvider.GetRequiredService<ICurrencyEvent>();
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
                                case AnalysedComponentType.HourlyMarketCap:
                                case AnalysedComponentType.DailyMarketCap:
                                    var obtainedComponent = _analysedComponentEvent
                                        .GetAllByCurrencyType((long) entity.CurrencyTypeId, true, 
                                            0, dataTimespan.Milliseconds)
                                        .SingleOrDefault(ac => ac.ComponentType.Equals(entity.ComponentType));
                                    
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
                                        .Where(ac => ac.ComponentType.Equals(AnalysedComponentType.MarketCap))
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
                                                        (marketCapByCurrencies[ac.Currency.Abbreviation] + val) / 2;
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
                                .SingleOrDefault(ac => ac.DeletedAt == null && ac.IsEnabled
                                                                            && ac.ComponentType
                                                                                .Equals(AnalysedComponentType.CurrentAveragePrice)
                                                                            && !string.IsNullOrEmpty(ac.Value)
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
                                    var marketCap = circuSupply
                                                    * averagePrice;

                                    if (!decimal.Zero.Equals(marketCap))
                                    {
                                        return _analysedComponentService.UpdateValue(entity.Id, marketCap.ToString());
                                    }
                                }
                            }
                        }
                        // Request-based Market Cap
                        else
                        {
                            var circuSupply = _currencyEvent.GetCirculatingSupply(entity);
                            analysedComponents = _analysedComponentEvent.GetAllByCorrelation(entity.Id);

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
                                    return _analysedComponentService.UpdateValue(entity.Id, marketCap.ToString());
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