using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyEvent
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ITickerEvent _tickerEvent;

        public CurrencyEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ICurrencyPairEvent currencyPairEvent, ITickerEvent tickerEvent)
            : base(logger, unitOfWork)
        {
            _currencyPairEvent = currencyPairEvent;
            _tickerEvent = tickerEvent;
        }

        public Currency Get(long id, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

            return query
                .SingleOrDefault(c => c.Id.Equals(id));
        }

        /// <summary>
        /// Public API for obtaining data specific to a currency with its abbreviation.
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public Currency GetCurrencyByAbbreviation(string abbreviation, bool track = false)
        {
            // First obtain all 'ABBRV' objects first, 
//            var currency = _unitOfWork.GetRepository<Currency>()
//                .GetQueryable()
//                .AsNoTracking()
//                .Where(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase)
//                            && c.DeletedAt == null && c.IsEnabled)
//                .Include(c => c.AnalysedComponents)
//                .Include(c => c.CurrencySource)
//                .Include(c => c.CurrencyPairSourceCurrencies)
//                .ThenInclude(pcp => pcp.Currency)
//                .Include(c => c.CurrencyCurrencyPairs)
//                .ThenInclude(pcp => pcp.CurrencyPair)
//                .ThenInclude(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.AnalysedComponents)
//                .Include(c => c.CurrencyCurrencyPairs)
//                .ThenInclude(pcp => pcp.CurrencyPair)
//                .ThenInclude(cp => cp.CurrencyPairRequests)
//                .ThenInclude(cpr => cpr.RequestComponents)
//                .Include(c => c.CurrencyRequests)
//                .ThenInclude(cr => cr.RequestComponents)
//                .ToList();
//
//            // Filter it
//            var cpACs = currency
//                .SelectMany(c => c.CurrencyCurrencyPairs)
//                .Select(pcp => pcp.CurrencyPair)
//                .Where(cp => cp.CounterCurrency.Equals(CoreConstants.GenericCounterCurrency,
//                    StringComparison.InvariantCultureIgnoreCase))
//                .SelectMany(cp => cp.CurrencyPairRequests)
//                .SelectMany(cpr => cpr.AnalysedComponents
//                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled))
//                .ToList();
//
//            if (currency.Count > 0)
//            {
//                var result = new AbbrvUniqueCurrencyResponse(currency.FirstOrDefault());
//
//                // DTO this sitch
//                foreach (var similarCurr in currency)
//                {
//                    // Currency-based ACs
//                    foreach (var aComp in similarCurr.AnalysedComponents)
//                    {
//                        if (!result.AnalysedComponents
//                            .Any(ac => ac.Id.Equals(aComp.Id)))
//                        {
//                            result.AnalysedComponents.Add(aComp);
//                        }
//                    }
//
//                    // Currency Pair-based ACs
//                    foreach (var cpAComp in cpACs)
//                    {
//                        if (!result.AnalysedComponents
//                            .Any(ac => ac.Id.Equals(cpAComp.Id)))
//                        {
//                            result.AnalysedComponents.Add(cpAComp);
//                        }
//                    }
//
//                    foreach (var cReq in similarCurr.CurrencyRequests)
//                    {
//                        if (!result.CurrencyRequests.Any(cr => cr.Id.Equals(cReq.Id)))
//                        {
//                            result.CurrencyRequests.Add(cReq);
//                        }
//                    }
//
//                    foreach (var pCPair in similarCurr.CurrencyCurrencyPairs
//                        .Where(ccp => ccp.Currency.Abbreviation
//                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase)))
//                    {
//                        // Form the TickerPair abbreviation first
//                        var tickerPairStr = pCPair.Currency.Abbreviation +
//                                            similarCurr.CurrencyCurrencyPairs
//                                                .SingleOrDefault(ccp => ccp.Currency.Abbreviation
//                                                                            .Equals(ccp.CurrencyPair.CounterCurrency,
//                                                                                StringComparison
//                                                                                    .InvariantCultureIgnoreCase)
//                                                                        && ccp.CurrencyPairId.Equals(
//                                                                            pCPair.CurrencyPairId))
//                                                ?.Currency.Abbreviation;
//
//                        // Make sure tickerPairStr is not a solo piece of shit
//                        if (!tickerPairStr.Equals(pCPair.Currency.Abbreviation,
//                            StringComparison.InvariantCultureIgnoreCase))
//                        {
//                            // Make sure this is a main currency and that the TickerPairs collection
//                            // does not contain this TickerPair yet.
//                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation.Equals(tickerPairStr,
//                                StringComparison.InvariantCultureIgnoreCase)))
//                            {
//                                // Since all is good, let's toss it in
//                                result.TickerPairs.Add(new CondensedTickerPair
//                                {
//                                    PairAbbreviation = tickerPairStr,
//                                    Sources = new List<SourceResponse>
//                                    {
//                                        new SourceResponse
//                                        {
//                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                            Name = similarCurr.CurrencySource.Name
//                                        }
//                                    }
//                                });
//                            }
//                            // Since the TickerPair already exists,
//                            else
//                            {
//                                // Pull the TickerPair
//                                var tickerPair = result.TickerPairs.SingleOrDefault(tp =>
//                                    tp.PairAbbreviation.Equals(tickerPairStr,
//                                        StringComparison.InvariantCultureIgnoreCase));
//
//                                // Check if the source exists first
//                                if (tickerPair.Sources.Any(s => s.Abbreviation
//                                    .Equals(similarCurr.CurrencySource.Abbreviation,
//                                        StringComparison.InvariantCultureIgnoreCase)))
//                                {
//                                    // It does, so let's ignore.
//                                    // If it reaches here it means that we have a duplicate.
//                                }
//                                else
//                                {
//                                    // Does not, add it in.
//                                    tickerPair.Sources.Add(new SourceResponse
//                                    {
//                                        Abbreviation = similarCurr.Abbreviation,
//                                        Name = similarCurr.Name
//                                    });
//                                }
//                            }
//                        }
//                    }
//
//                    var counterPCPs = _currencyCurrencyPairEvent
//                        .ObtainCounterCurrencyPairs(similarCurr.CurrencyCurrencyPairs);
//
//                    if (counterPCPs != null && counterPCPs.Count > 0)
//                    {
//                        foreach (var pCPair in similarCurr.CurrencyCurrencyPairs)
//                        {
//                            var tickerPairStr = pCPair.Currency.Abbreviation +
//                                                counterPCPs
//                                                    .SingleOrDefault(pcp =>
//                                                        pcp.CurrencyPairId.Equals(pCPair.CurrencyPairId))
//                                                    ?.Currency.Abbreviation;
//
//                            if (!result.TickerPairs.Any(tp => tp.PairAbbreviation
//                                .Equals(tickerPairStr, StringComparison.InvariantCultureIgnoreCase)))
//                            {
//                                result.TickerPairs.Add(new CondensedTickerPair
//                                {
//                                    PairAbbreviation = tickerPairStr,
//                                    Sources = new List<SourceResponse>
//                                    {
//                                        new SourceResponse
//                                        {
//                                            Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                            Name = similarCurr.CurrencySource.Name
//                                        }
//                                    }
//                                });
//                            }
//                            else
//                            {
//                                // Add since it exists
//                                result.TickerPairs.SingleOrDefault(tp => tp.PairAbbreviation
//                                                                             .Equals(tickerPairStr,
//                                                                                 StringComparison
//                                                                                     .InvariantCultureIgnoreCase)
//                                                                         && !tp.Sources.Any(s => s.Abbreviation
//                                                                             .Equals(
//                                                                                 similarCurr.CurrencySource
//                                                                                     .Abbreviation,
//                                                                                 StringComparison
//                                                                                     .InvariantCultureIgnoreCase)))
//                                    ?.Sources.Add(new SourceResponse
//                                    {
//                                        Abbreviation = similarCurr.CurrencySource.Abbreviation,
//                                        Name = similarCurr.CurrencySource.Name
//                                    });
//                            }
//                        }
//                    }
//
//                    if (!result.CurrencySources.Any(cs => cs.Id.Equals(similarCurr.CurrencySourceId)))
//                    {
//                        result.CurrencySources.Add(similarCurr.CurrencySource);
//                    }
//                }
//
//                return result;
//            }

            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query = query.Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

            return query
                .SingleOrDefault(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase));
        }

        public decimal GetCirculatingSupply(AnalysedComponent analysedComponent)
        {
            // If its a currency-based ac
            if (analysedComponent.CurrencyId != null && analysedComponent.CurrencyId > 0)
            {
                // Obtain the currency that is required
                var curr = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .SingleOrDefault(c => c.Id.Equals(analysedComponent.CurrencyId)
                                          && c.IsEnabled && c.DeletedAt == null);

                // Safetynet
                if (curr == null)
                {
                    return decimal.MinusOne;
                }

                // Denomination safetynet
                if (curr.Denominations <= 0)
                {
                    curr.Denominations = 1; // Neutraliser
                }

                // Then, we obtain the circulating supply.

                // TODO: Validate with multiple sources.
                
                var reqComp = _unitOfWork.GetRepository<CurrencyRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cr => cr.DeletedAt == null && cr.IsEnabled
                                 && cr.CurrencyId.Equals(curr.Id))
                    .Include(cp => cp.Currency)
                    .Include(cpr => cpr.RequestComponents)
                    // Obtain only the circulating supply
                    .SelectMany(cpr => cpr.RequestComponents
                        .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                          && rc.ComponentType.Equals(ComponentType.Circulating_Supply)
                                                          && !string.IsNullOrEmpty(rc.Value)))
                    .FirstOrDefault();

                return reqComp != null
                    ? decimal.Parse(reqComp.Value ?? "0") /
                      (reqComp.IsDenominated ? (decimal) Math.Pow(10, curr.Denominations) : decimal.One)
                    : decimal.Zero;
            }
            else if (analysedComponent.CurrencyPairId != null && analysedComponent.CurrencyPairId > 0)
                // It means that this is a currency pair 
            {
                #if DEBUG
                try
                {
                    var qTest = _unitOfWork.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Where(cp => cp.Id.Equals(analysedComponent.CurrencyPairId)
                                     && cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.Source)
                        .ThenInclude(s => s.SourceCurrencies)
                        .ThenInclude(sc => sc.Currency)
                        .ThenInclude(c => c.CurrencyRequests)
                        .ThenInclude(cr => cr.RequestComponents)
                        .Where(cp => cp.Source != null && cp.Source.SourceCurrencies != null)
                        // Obtain the main currency
                        .Select(cp => decimal.Parse(cp.Source
                                                        .SourceCurrencies
                                                        .SingleOrDefault(sc => 
                                                            sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv)
                                                            && sc.Currency.CurrencyRequests != null)
                                                        .Currency
                                // Traverse to the request
                                .CurrencyRequests
                                .Where(cr => cr.RequestComponents != null && cr.RequestComponents.Count > 0
                                                                          && cr.RequestComponents.Any(rc => rc.ComponentType
                                                                              .Equals(ComponentType.Circulating_Supply)))
                                .Select(cr => cr.RequestComponents
                                    .FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled))
                                .FirstOrDefault()
                                .Value ?? "-1"))
                        .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                #endif
                
                return _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.Id.Equals(analysedComponent.CurrencyPairId)
                                 && cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.Source)
                    .ThenInclude(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency)
                    .ThenInclude(c => c.CurrencyRequests)
                    .ThenInclude(cr => cr.RequestComponents)
                    // Obtain the main currency
                    .Select(cp => decimal.Parse(cp.Source
                                                    .SourceCurrencies
                                                    .SingleOrDefault(sc => 
                                                        sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv)
                                                        && sc.Currency.CurrencyRequests != null)
                                                    .Currency
                                                    // Traverse to the request
                                                    .CurrencyRequests
                                                    .Where(cr => cr.RequestComponents != null && cr.RequestComponents.Count > 0
                                                                                              && cr.RequestComponents.Any(rc => rc.ComponentType
                                                                                                  .Equals(ComponentType.Circulating_Supply)))
                                                    .Select(cr => cr.RequestComponents
                                                        .FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled))
                                                    .FirstOrDefault()
                                                    .Value ?? "-1"))
                    .FirstOrDefault();
            }

            return decimal.MinusOne;
        }

        public ICollection<Currency> GetAll(bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyRequests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }
        
        public ICollection<Currency> GetAllNonDeleted(bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null);

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.CurrencyRequests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }
        
        public ICollection<CurrencyDTO> GetAllDTO()
        {
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.CurrencySources)
                .Select(c => new CurrencyDTO
                {
                    Id = c.Id,
                    CurrencyType = c.CurrencyType,
                    LogoPath = c.LogoPath,
                    Abbreviation = c.Abbreviation,
                    SourceCount = c.CurrencySources.Count,
                    Slug = c.Slug,
                    Name = c.Name,
                    Description = c.Description,
                    DenominationName = c.DenominationName,
                    IsEnabled = c.IsEnabled
                }).ToList();
        }

        public ICollection<DetailedCurrencyResponse> GetAllDetailed(string typeShortForm = "CRYPTO",
            int index = 0, int daysOfData = 7)
        {
            var currencies = _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.TypeShortForm.Equals(typeShortForm, StringComparison.InvariantCultureIgnoreCase))
                .Include(ct => ct.Currencies)
                .ThenInclude(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .SelectMany(ct => ct.Currencies
                    .Where(c => c.DeletedAt == null && c.IsEnabled
                                && c.AnalysedComponents.Count > 0
                                && c.AnalysedComponents.Any(ac => ac.ComponentType.Equals(AnalysedComponentType.MarketCap)))
                    .OrderByDescending(c => decimal.Parse(c.AnalysedComponents
                        .SingleOrDefault(ac => ac.ComponentType == AnalysedComponentType.MarketCap).Value))
                    .Skip(index * 100)
                    .Take(100)
                    .Select(c => new Currency
                    {
                        Id = c.Id,
                        CurrencyTypeId = c.CurrencyTypeId,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName,
                        LogoPath = c.LogoPath,
                        AnalysedComponents = c.AnalysedComponents
                            .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
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
                                    .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                                        && ahi.HistoricDateTime >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysOfData)))
                                    .OrderByDescending(ahi => ahi.HistoricDateTime)
                                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                                    .ToList()
                            })
                            .ToList()
                    }))
                .ToList();

            var res = new List<DetailedCurrencyResponse>();

            foreach (var currency in currencies)
            {
                if (currency.AnalysedComponents != null && currency.AnalysedComponents.Count > 0)
                {
                    res.Add(new DetailedCurrencyResponse(currency, 
                        _tickerEvent.GetCurrencyTickerPairs(currency.Abbreviation)));
                }
            }
            
            return res.OrderByDescending(dcr => dcr.MarketCap).ToList();
        }

        public DetailedCurrencyResponse GetDetailedById(long currencyId, ICollection<AnalysedComponentType> componentTypes)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Id.Equals(currencyId))
                .Include(cp => cp.AnalysedComponents
                    .Where(ac => componentTypes.Contains(ac.ComponentType)))
                    .ThenInclude(ac => ac.AnalysedHistoricItems)
                .SingleOrDefault();

            if (query == null) return null;

            return new DetailedCurrencyResponse(query, 
                _tickerEvent.GetCurrencyTickerPairs(query.Abbreviation));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="componentTypes"></param>
        /// <returns></returns>
        public DetailedCurrencyResponse GetDetailedBySlug(string slug,
            ICollection<AnalysedComponentType> componentTypes)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.AnalysedComponents
                    .Where(ac => componentTypes.Contains(ac.ComponentType)))
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .SingleOrDefault();

            if (query == null) return null;

            return new DetailedCurrencyResponse(query, 
                _tickerEvent.GetCurrencyTickerPairs(query.Abbreviation));
        }

        public bool Any(CreateCurrency createCurrency)
        {
            if (createCurrency != null && createCurrency.IsValid())
            {
                return _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(createCurrency.Abbreviation)).Any();
            }

            return false;
        }

        public IEnumerable<Currency> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyRequests)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyProperties)
                    .Include(c => c.CurrencyType);
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled);
            }
        }

        /// <summary>
        /// Gets all active.
        /// 
        /// </summary>
        /// <param name="includeNested"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        name = c.Name
                    });
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        name = c.Name
                    });
            }
        }

        /// <summary>
        /// Gets all active distinctively.
        /// 
        /// This is to prevent duplicates for displaying purposes. The user would normally
        /// be prompted after selecting a choice from this result before creating
        /// a final decision.
        /// </summary>
        /// <returns>The all active distinct.</returns>
        /// <param name="includeNested">If set to <c>true</c> include nested.</param>
        public IEnumerable<dynamic> GetAllActiveDistinctObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        Name = c.Name
                    });
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        Name = c.Name
                    });
            }
        }
    }
}