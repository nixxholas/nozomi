using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedComponentEvent : BaseEvent<AnalysedComponentEvent, NozomiDbContext>, IAnalysedComponentEvent
    {
        public AnalysedComponentEvent(ILogger<AnalysedComponentEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public AnalysedComponent Get(long id, bool track = false, int index = 0)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ac => ac.Id.Equals(id));
            
            if (track)
            {
                query
                    .Include(ac => ac.Currency)
                    .Include(ac => ac.CurrencyPair)
                    .Include(ac => ac.CurrencyType)
                    .Include(ac => ac.AnalysedHistoricItems);

                return query
                    .Select(ac => new AnalysedComponent
                    {
                        Id = ac.Id,
                        ComponentType = ac.ComponentType,
                        CurrencyType = ac.CurrencyType,
                        CurrencyTypeId = ac.CurrencyTypeId,
                        Value = ac.Value,
                        IsDenominated = ac.IsDenominated,
                        Delay = ac.Delay,
                        UIFormatting = ac.UIFormatting,
                        AnalysedHistoricItems = ac.AnalysedHistoricItems
                            .OrderByDescending(ahi => ahi.HistoricDateTime)
                            .Skip(index * 200)
                            .Take(200)
                            .ToList()
                    })
                    .SingleOrDefault();
            }

            return query.SingleOrDefault();
        }

        public IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false, int index = 0)
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
                    .Include(ac => ac.Currency)
                    .Include(ac => ac.CurrencyPair)
                    .Include(ac => ac.CurrencyType);
                
                return query
                    .Select(ac => new AnalysedComponent
                    {
                        Id = ac.Id,
                        ComponentType = ac.ComponentType,
                        CurrencyType = ac.CurrencyType,
                        CurrencyTypeId = ac.CurrencyTypeId,
                        Value = ac.Value,
                        IsDenominated = ac.IsDenominated,
                        Delay = ac.Delay,
                        UIFormatting = ac.UIFormatting,
                        AnalysedHistoricItems = ac.AnalysedHistoricItems
                            .OrderByDescending(ahi => ahi.HistoricDateTime)
                            .Skip(index * 200)
                            .Take(200)
                            .ToList()
                    });
            }

            return query;
        }

        public IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false)
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
                    .Include(ac => ac.Currency)
                    .Include(ac => ac.CurrencyPair)
                    .Include(ac => ac.CurrencyType);
            }

            return query
                .Skip(index * 50)
                .Take(50);
        }

        public ICollection<AnalysedComponent> GetAllCurrencyTypeAnalysedComponents(int index = 0, bool filter = false, bool track = false)
        {
            var query = _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking();

            if (filter)
            {
                query = query.Where(ct => ct.DeletedAt == null && ct.IsEnabled);
            }

            if (track)
            {
                query = query.Include(ct => ct.AnalysedComponents)
                    .ThenInclude(ac => ac.AnalysedHistoricItems);
            }
            else
            {
                query = query.Include(ct => ct.AnalysedComponents);
            }

            return query
                .SelectMany(ct => ct.AnalysedComponents
                    .Where(ac => ac.IsEnabled && ac.DeletedAt == null))
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    CurrencyType = ac.CurrencyType,
                    CurrencyTypeId = ac.CurrencyTypeId,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    Delay = ac.Delay,
                    UIFormatting = ac.UIFormatting,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                        .OrderByDescending(ahi => ahi.HistoricDateTime)
                        .Skip(index * 200)
                        .Take(200)
                        .ToList()
                })
                .ToList();
        }

        /// <summary>
        /// Obtains all Analysed Components relevant to the currency in question based on the generic counter currency.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="ensureValid"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        public ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool ensureValid = false, bool track = false)
        {
            // First, obtain the currency in question
            var qCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Id.Equals(currencyId));

            if (qCurrency.SingleOrDefault() == null) return null;

            qCurrency = qCurrency.Include(c => c.AnalysedComponents);

            if (ensureValid)
            {
                qCurrency = qCurrency.Where(c => c.DeletedAt == null && c.IsEnabled);
            }

            if (track)
            {
                qCurrency = qCurrency.Include(c => c.AnalysedComponents)
                    .ThenInclude(ac => ac.AnalysedHistoricItems);
            }

            return qCurrency
                .SelectMany(c => c.AnalysedComponents)
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    Delay = ac.Delay,
                    UIFormatting = ac.UIFormatting,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                })
                .ToList();


//            // Then we return
//            var finalQuery = _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .AsNoTracking()
//                // Immediately filter first
//                .Where(cp =>
//                    // Make sure the main currencies are identical
//                    cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.CurrencyPair.MainCurrency
//                            .Equals(ccp.Currency.Abbreviation)).Currency.Abbreviation
//                        .Equals(qCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase));
//            
//            #if DEBUG
//            // Debug and see if the correct currencies are present.
//            var correctCPairList = finalQuery.ToList();
//            #endif
//
//            if (ensureValid)
//            {
//                finalQuery = finalQuery
//                    .Where(cp => cp.IsEnabled && cp.DeletedAt == null);
//            }
//
//            if (!string.IsNullOrEmpty(counterCurrency))
//            {
//                finalQuery = finalQuery.Where(cp =>
//                    cp.CounterCurrency.Equals(counterCurrency, StringComparison.InvariantCultureIgnoreCase));
//            }
//            
//            #if DEBUG
//            var filteredCPairList = finalQuery.ToList();
//            #endif
//
//            var analysedComponents = finalQuery
//                .Include(cp => cp.CurrencyPairCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .Include(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .ThenInclude(rc => rc.AnalysedHistoricItems)
//                .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
//                                                                              && cpr.AnalysedComponents
//                                                                                  .Any(ac =>
//                                                                                      ac.IsEnabled && ac.DeletedAt ==
//                                                                                                   null
//                                                                                                   && ac.AnalysedHistoricItems.Count > 0)))
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.AnalysedComponents);
//
//#if DEBUG
//            if (currencyId.Equals(4))
//            {
//                var test = analysedComponents.ToList();
//                Console.WriteLine("Banf");
//            }
//#endif
//            
//            if (!finalQuery.Any()) return new List<AnalysedComponent>();
//
//            if (!track)
//            {
//                return analysedComponents
//                    .Select(ac => new AnalysedComponent
//                    {
//                        Id = ac.Id,
//                        ComponentType = ac.ComponentType,
//                        Value = ac.Value,
//                        Delay = ac.Delay,
//                        RequestId = ac.RequestId,
//                        CurrencyId = ac.CurrencyId
//                    })
//                    .ToList();
//            }
//            
//            return analysedComponents
//                .Select(ac => new AnalysedComponent
//                {
//                    AnalysedHistoricItems = ac.AnalysedHistoricItems,
//                    Id = ac.Id,
//                    ComponentType = ac.ComponentType,
//                    Value = ac.Value,
//                    Delay = ac.Delay,
//                    RequestId = ac.RequestId,
//                    CurrencyId = ac.CurrencyId
//                })
//                .ToList();
        }

        public ICollection<AnalysedComponent> GetTickerPairComponentsByCurrency(long currencyId, bool ensureValid = false, bool track = false)
        {
            var cPairs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                // Make sure the source has such currency
                .Where(cp => cp.Source.SourceCurrencies.Any(sc => sc.CurrencyId.Equals(currencyId)
                                                                  // And that the main currency abbreviation matches
                                                                  // the currency's abbreviation
                                                                  && sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv)));

            if (ensureValid)
            {
                cPairs = cPairs.Where(cp => cp.DeletedAt == null && cp.IsEnabled);
            }

            cPairs = cPairs
                .Include(cp => cp.AnalysedComponents);

            if (track)
            {
                cPairs = cPairs.Include(c => c.AnalysedComponents)
                    .ThenInclude(ac => ac.AnalysedHistoricItems);
            }

            return cPairs
                .SelectMany(cp => cp.AnalysedComponents)
                .Select(ac => new AnalysedComponent
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    Delay = ac.Delay,
                    UIFormatting = ac.UIFormatting,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems,
                    CurrencyPairId = ac.CurrencyPairId,
                    CurrencyPair = ac.CurrencyPair
                })
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false)
        {
            if (currencyTypeId > 0)
            {
                var components = _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .Where(ac => ac.CurrencyTypeId.Equals(currencyTypeId));

                if (track)
                {
                    components
                        .Include(ac => ac.CurrencyType)
                        .ThenInclude(ct => ct.Currencies)
                        .ThenInclude(c => c.AnalysedComponents)
                        .Include(ac => ac.AnalysedHistoricItems);
                }

                return components.ToList();
            }

            return null;
        }

        public ICollection<AnalysedComponent> GetAllCurrencyComponentsByType(long currencyTypeId, bool track = false)
        {
            if (currencyTypeId > 0)
            {
                var components = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .Where(c => c.CurrencyTypeId.Equals(currencyTypeId))
                    .Include(c => c.AnalysedComponents);

                if (track)
                {
                    components.ThenInclude(ac => ac.AnalysedHistoricItems);
                }

                return components
                    .SelectMany(c => c.AnalysedComponents)
                    .Select(ac => new AnalysedComponent
                    {
                        Id = ac.Id,
                        ComponentType = ac.ComponentType,
                        Value = ac.Value,
                        IsDenominated = ac.IsDenominated,
                        Delay = ac.Delay,
                        UIFormatting = ac.UIFormatting,
                        AnalysedHistoricItems = ac.AnalysedHistoricItems,
                        CurrencyId = ac.CurrencyId,
                        Currency = ac.Currency
                    })
                    .ToList();
            }

            return null;
        }

        public ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false)
        {
            var aComp = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

            if (aComp == null) return new List<AnalysedComponent>();

            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ac => (aComp.CurrencyPairId != null && ac.CurrencyPairId.Equals(aComp.CurrencyPairId)) 
                             || (aComp.CurrencyId != null && ac.CurrencyId.Equals(aComp.CurrencyId))
                             || (aComp.CurrencyTypeId != null && ac.CurrencyTypeId.Equals(aComp.CurrencyTypeId)));

            if (track)
            {
                query = query.Include(ac => ac.AnalysedHistoricItems);
            }

            return query
                .ToList();

//            // First, obtain the correlation PCPs
//            var correlPCPs = _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
//                .Include(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .Where(cp => cp.CurrencyPairRequests
//                    .Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
//                                                      // We can ignore disabled or deleted ACs, just using this 
//                                                      // to find the correlation
//                                                      && cpr.AnalysedComponents.Any(ac =>
//                                                          ac.Id.Equals(analysedComponentId))))
//                .Include(cp => cp.CurrencyPairCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .SelectMany(cp => cp.CurrencyPairCurrencies)
//                .Select(pcp => new CurrencyPairSourceCurrency
//                {
//                    CurrencyId = pcp.CurrencyId,
//                    CurrencyPairId = pcp.CurrencyPairId,
//                    Currency = pcp.Currency,
//                    CurrencyPair = pcp.CurrencyPair
//                })
//                .ToList();
//
//            // Check if its a currency-based correlation
//            if (correlPCPs.Count < 2)
//            {
//                // Since it is currency based, let's take a look
//                var currency = _unitOfWork.GetRepository<Currency>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(c => c.DeletedAt == null && c.IsEnabled)
//                    .Include(c => c.AnalysedComponents)
//                    .FirstOrDefault(c => c.AnalysedComponents.Any(ac => ac.Id.Equals(analysedComponentId)));
//
//                if (currency != null)
//                {
//                    // Obtain all analysed components relevant to the generic counter currency
//                    var currencyACs = _unitOfWork.GetRepository<CurrencyPair>()
//                        .GetQueryable()
//                        .AsNoTracking()
//                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
//                        .Include(cp => cp.CurrencyPairCurrencies)
//                        .ThenInclude(pcp => pcp.Currency)
//                        .Where(cp =>
//                            cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                                    .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
//                                .Currency.Abbreviation
//                                .Equals(currency.Abbreviation, StringComparison.InvariantCultureIgnoreCase)
//                            // Counter currency is the generic counter currency
//                            && cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                                    .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase))
//                                .Currency.Abbreviation
//                                .Contains(CoreConstants.GenericCounterCurrency,
//                                    StringComparison.InvariantCultureIgnoreCase))
//                        .Include(cp => cp.CurrencyPairRequests)
//                        .ThenInclude(cpr => cpr.AnalysedComponents);
//
//                    if (track)
//                    {
//                        return currencyACs
//                            .ThenInclude(ac => ac.AnalysedHistoricItems)
//                            .SelectMany(cp => cp.CurrencyPairRequests)
//                            .SelectMany(cpr => cpr.AnalysedComponents)
//                            .Select(ac => new AnalysedComponent
//                            {
//                                AnalysedHistoricItems = ac.AnalysedHistoricItems,
//                                Id = ac.Id,
//                                ComponentType = ac.ComponentType,
//                                Value = ac.Value,
//                                Delay = ac.Delay,
//                                RequestId = ac.RequestId,
//                                CurrencyId = ac.CurrencyId
//                            })
//                            .ToList();
//                    }
//                    
//                    return currencyACs
//                        .SelectMany(cp => cp.CurrencyPairRequests)
//                        .SelectMany(cpr => cpr.AnalysedComponents)
//                        .Select(ac => new AnalysedComponent
//                        {
//                            Id = ac.Id,
//                            ComponentType = ac.ComponentType,
//                            Value = ac.Value,
//                            Delay = ac.Delay,
//                            RequestId = ac.RequestId,
//                            CurrencyId = ac.CurrencyId
//                        })
//                        .ToList();
//                }
//                
//                // Empty since nothing
//                return new List<AnalysedComponent>();
//            }
//
//            // Then we return
//            var finalQuery = _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
//                .Include(cp => cp.CurrencyPairCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .Where(cp =>
//                    // Make sure the main currencies are identical
//                    cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
//                        .Currency.Abbreviation
//                        .Equals(correlPCPs.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))
//                            .Currency.Abbreviation)
//                    // Make sure the counter currencies are identical
//                    && cp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase)).Currency.Abbreviation
//                        .Equals(correlPCPs.FirstOrDefault(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase)).Currency.Abbreviation))
//                .Include(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .ThenInclude(ac => ac.AnalysedHistoricItems)
//                .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.IsEnabled && cpr.DeletedAt == null
//                                                                              && cpr.AnalysedComponents
//                                                                                  .Any(ac =>
//                                                                                      ac.IsEnabled && ac.DeletedAt ==
//                                                                                                   null)))
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.AnalysedComponents);
//
//            if (!finalQuery.Any()) return new List<AnalysedComponent>();
//
//            if (!track)
//            {
//                return finalQuery
//                    .Select(ac => new AnalysedComponent
//                    {
//                        Id = ac.Id,
//                        ComponentType = ac.ComponentType,
//                        Value = ac.Value,
//                        Delay = ac.Delay,
//                        RequestId = ac.RequestId,
//                        CurrencyId = ac.CurrencyId
//                    })
//                    .ToList();
//            }
//
//            return finalQuery
//                .Where(ac => ac.AnalysedHistoricItems != null)
//                .Select(ac => new AnalysedComponent
//                {
//                    AnalysedHistoricItems = ac.AnalysedHistoricItems,
//                    Id = ac.Id,
//                    ComponentType = ac.ComponentType,
//                    Value = ac.Value,
//                    Delay = ac.Delay,
//                    RequestId = ac.RequestId,
//                    CurrencyId = ac.CurrencyId
//                })
//                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.Id.Equals(currencyPairId))
                .Include(r => r.AnalysedComponents);

            if (track)
            {
                 query.ThenInclude(ac => ac.AnalysedHistoricItems);
            }

            return query.SelectMany(r => r.AnalysedComponents).ToList();
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
                ?.Abbreviation;
        }
    }
}