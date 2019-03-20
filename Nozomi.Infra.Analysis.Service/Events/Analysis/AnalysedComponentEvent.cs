using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis
{
    public class AnalysedComponentEvent : BaseEvent<AnalysedComponentEvent, NozomiDbContext>, IAnalysedComponentEvent
    {
        public AnalysedComponentEvent(ILogger<AnalysedComponentEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public void ConvertToGenericCurrency(ICollection<AnalysedComponent> analysedComponents)
        {
            if (analysedComponents != null && analysedComponents.Count > 0)
            {
                // Iterate all of the reqcomps for conversion
                foreach (var analysedComp in analysedComponents)
                {
                    // Obtain the current req com's counter currency first
                    var counterCurr = _unitOfWork.GetRepository<Currency>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Include(c => c.PartialCurrencyPairs)
                        .ThenInclude(c => c.Currency)
                        .Include(c => c.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.CurrencyPair)
                        .ThenInclude(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.AnalysedComponents)
                        .SingleOrDefault(c => c.PartialCurrencyPairs
                                // Make sure we're not converting if we don't have to.
                            .Where(pcp => !pcp.IsMain 
                                          && !pcp.Currency.Abbrv.Equals(CoreConstants.GenericCounterCurrency,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Select(pcp => pcp.CurrencyPair)
                            .SelectMany(cp => cp.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .Any(rc => rc.Id.Equals(analysedComp.Id)));

                    // Null check
                    if (counterCurr != null)
                    {
                        // Obtain the conversion rate
                        var conversionRate = _unitOfWork
                            .GetRepository<PartialCurrencyPair>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(pcp => pcp.IsMain &&
                                          pcp.Currency.Abbrv.Equals(counterCurr.Abbrv,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Where(pcp => !pcp.IsMain && 
                                          pcp.Currency.Abbrv.Equals(CoreConstants.GenericCounterCurrency,
                                              StringComparison.InvariantCultureIgnoreCase))
                            .Include(pcp => pcp.CurrencyPair)
                            .ThenInclude(cp => cp.CurrencyPairRequests)
                            .ThenInclude(cpr => cpr.AnalysedComponents)
                            .SelectMany(pcp => pcp.CurrencyPair.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .FirstOrDefault(ac => ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));

                        if (conversionRate != null && decimal.TryParse(conversionRate.Value, out var conversionVal))
                        {
                            // Since we've gotten the conversion rate, let's convert.
//                            analysedComp.RequestComponentDatum.Value = (decimal.Parse(analysedComp.RequestComponentDatum.Value)
//                                                                  * conversionVal).ToString(CultureInfo.InvariantCulture);
                            foreach (var analysedCompAnalysedHistoricItem in analysedComp.AnalysedHistoricItems)
                            {
                                analysedCompAnalysedHistoricItem.Value =
                                    (decimal.Parse(analysedCompAnalysedHistoricItem.Value)
                                     * conversionVal).ToString(CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning($" Request Component: {analysedComp.Id} does not have a counter currency.");
                    }
                }
            }
        }

        public IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking();

            if (filter)
            {
                query.Where(ac => ac.IsEnabled && ac.DeletedAt == null
                                  &&  (ac.Delay == 0 || 
                                       DateTime.UtcNow > ac.ModifiedAt.AddMilliseconds(ac.Delay).ToUniversalTime()));
            }
            
            if (track)
            {
                query
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Include(ac => ac.Request)
                    .ThenInclude(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .ThenInclude(rcd => rcd.RcdHistoricItems);
            }

            return query;
        }

        public ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool track = false)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(c => c.Id.Equals(currencyId));

            if (qCurrency == null) return null;

            // Then we return
            var finalQuery = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp =>
                    // Make sure the main currencies are identical
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                        .Equals(qCurrency.Abbrv, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .ThenInclude(rc => rc.AnalysedHistoricItems)
                .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                                              && cpr.AnalysedComponents
                                                                                  .Any(ac =>
                                                                                      ac.IsEnabled && ac.DeletedAt ==
                                                                                                   null
                                                                                                   && ac.AnalysedHistoricItems.Count > 0)))
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.AnalysedComponents);

            if (!finalQuery.Any()) return new List<AnalysedComponent>();

            if (!track)
            {
                return finalQuery
                    .Select(ac => new AnalysedComponent
                    {
                        Id = ac.Id,
                        ComponentType = ac.ComponentType,
                        Value = ac.Value,
                        Delay = ac.Delay,
                        RequestId = ac.RequestId,
                        CurrencyId = ac.CurrencyId
                    })
                    .ToList();
            }
            
            return finalQuery
                .Select(ac => new AnalysedComponent
                {
                    AnalysedHistoricItems = ac.AnalysedHistoricItems,
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    Delay = ac.Delay,
                    RequestId = ac.RequestId,
                    CurrencyId = ac.CurrencyId
                })
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false)
        {
            // First, obtain the correlation PCPs
            var correlPCPs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .Where(cp => cp.CurrencyPairRequests
                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
                                                      // We can ignore disabled or deleted ACs, just using this 
                                                      // to find the correlation
                                                      && cpr.AnalysedComponents.Any(ac =>
                                                          ac.Id.Equals(analysedComponentId))))
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .SelectMany(cp => cp.PartialCurrencyPairs)
                .Select(pcp => new PartialCurrencyPair
                {
                    CurrencyId = pcp.CurrencyId,
                    CurrencyPairId = pcp.CurrencyPairId,
                    Currency = pcp.Currency,
                    IsMain = pcp.IsMain
                })
                .ToList();

            // Check if its a currency-based correlation
            if (correlPCPs.Count < 2)
            {
                // Since it is currency based, let's take a look
                var currency = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Include(c => c.AnalysedComponents)
                    .FirstOrDefault(c => c.AnalysedComponents.Any(ac => ac.Id.Equals(analysedComponentId)));

                if (currency != null)
                {
                    // Obtain all analysed components relevant to the generic counter currency
                    var currencyACs = _unitOfWork.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                        .Where(cp =>
                            cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                                .Equals(currency.Abbrv, StringComparison.InvariantCultureIgnoreCase)
                            // Counter currency is the generic counter currency
                            && cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv
                                .Equals(CoreConstants.GenericCounterCurrency,
                                    StringComparison.InvariantCultureIgnoreCase))
                        .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.AnalysedComponents);

                    if (track)
                    {
                        return currencyACs
                            .ThenInclude(ac => ac.AnalysedHistoricItems)
                            .SelectMany(cp => cp.CurrencyPairRequests)
                            .SelectMany(cpr => cpr.AnalysedComponents)
                            .Select(ac => new AnalysedComponent
                            {
                                AnalysedHistoricItems = ac.AnalysedHistoricItems,
                                Id = ac.Id,
                                ComponentType = ac.ComponentType,
                                Value = ac.Value,
                                Delay = ac.Delay,
                                RequestId = ac.RequestId,
                                CurrencyId = ac.CurrencyId
                            })
                            .ToList();
                    }
                    
                    return currencyACs
                        .SelectMany(cp => cp.CurrencyPairRequests)
                        .SelectMany(cpr => cpr.AnalysedComponents)
                        .Select(ac => new AnalysedComponent
                        {
                            Id = ac.Id,
                            ComponentType = ac.ComponentType,
                            Value = ac.Value,
                            Delay = ac.Delay,
                            RequestId = ac.RequestId,
                            CurrencyId = ac.CurrencyId
                        })
                        .ToList();
                }
                
                // Empty since nothing
                return new List<AnalysedComponent>();
            }

            // Then we return
            var finalQuery = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp =>
                    // Make sure the main currencies are identical
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv
                        .Equals(correlPCPs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv)
                    // Make sure the counter currencies are identical
                    && cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv
                        .Equals(correlPCPs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv))
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
                                                                              && cpr.AnalysedComponents
                                                                                  .Any(ac =>
                                                                                      ac.IsEnabled && ac.DeletedAt ==
                                                                                                   null)))
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.AnalysedComponents);

            if (!finalQuery.Any()) return new List<AnalysedComponent>();

            if (!track)
            {
                return finalQuery
                    .Select(ac => new AnalysedComponent
                    {
                        Id = ac.Id,
                        ComponentType = ac.ComponentType,
                        Value = ac.Value,
                        Delay = ac.Delay,
                        RequestId = ac.RequestId,
                        CurrencyId = ac.CurrencyId
                    })
                    .ToList();
            }

            return finalQuery
                .Where(ac => ac.AnalysedHistoricItems != null)
                .Select(ac => new AnalysedComponent
                {
                    AnalysedHistoricItems = ac.AnalysedHistoricItems,
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    Delay = ac.Delay,
                    RequestId = ac.RequestId,
                    CurrencyId = ac.CurrencyId
                })
                .ToList();
        }

        public string GetCurrencyAbbreviation(AnalysedComponent analysedComponent)
        {
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .Include(c => c.AnalysedComponents)
                .FirstOrDefault(c => c.AnalysedComponents
                    .Any(ac => ac.Id.Equals(analysedComponent.Id)))
                ?.Abbrv;
        }
    }
}