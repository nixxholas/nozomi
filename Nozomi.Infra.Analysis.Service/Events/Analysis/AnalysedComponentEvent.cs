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

        public ICollection<AnalysedComponent> GetAllByCurrency(long currencyId)
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
            
            return finalQuery
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                })
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId)
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
                .ToList();

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
                                                                                                   null
                                                                                                   && ac.AnalysedHistoricItems !=
                                                                                                   null)))
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.AnalysedComponents);

            if (!finalQuery.Any()) return new List<AnalysedComponent>();

            return finalQuery
                .Where(ac => ac.AnalysedHistoricItems != null)
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
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