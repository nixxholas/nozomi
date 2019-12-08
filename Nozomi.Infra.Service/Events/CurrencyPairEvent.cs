using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.CurrencyPair;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.CurrencyPair;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyPairEvent : BaseEvent<CurrencyPairEvent, NozomiDbContext>, ICurrencyPairEvent
    {
        public CurrencyPairEvent(ILogger<CurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyPair> GetAllByMainCurrency(string mainCurrencyAbbrv = CoreConstants.GenericCurrency)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                                                  && cp.MainCurrencyAbbrv.Equals(mainCurrencyAbbrv,
                                                      StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public IEnumerable<CurrencyPairViewModel> All(int page = 0, int itemsPerPage = 50, string sourceGuid = null, 
            string mainTicker = null, bool orderAscending = true, string orderingParam = "TickerPair")
        {
            if (itemsPerPage <= 0 || itemsPerPage > NozomiServiceConstants.CurrencyPairTakeoutLimit)
                itemsPerPage = NozomiServiceConstants.CurrencyPairTakeoutLimit;

            if (page < 0)
                page = 0;
            
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null && cp.SourceId > 0);

            if (!string.IsNullOrEmpty(mainTicker))
                query = query.Where(cp => cp.MainCurrencyAbbrv.Equals(mainTicker));
            
            if (!string.IsNullOrEmpty(sourceGuid) && Guid.TryParse(sourceGuid, out var parsedSourceGuid))
                query = query
                    .Include(c => c.Source)
                    .Where(cp => cp.Source.DeletedAt == null && cp.Source.IsEnabled && 
                                 cp.Source.Guid.Equals(parsedSourceGuid));

            switch (orderingParam.ToLower()) // Ignore case sensitivity
            {
                case "Type":
                    query = orderAscending ? query.OrderBy(cp => cp.CurrencyPairType) : 
                        query.OrderByDescending(cp => cp.CurrencyPairType);
                    break;
                case "SourceName":
                    query = orderAscending ? query
                        .Include(cp => cp.Source).OrderBy(cp => cp.Source.Name) : query
                        .Include(cp => cp.Source)
                        .OrderByDescending(cp => cp.Source.Name);
                    break;
                default: // Handle all cases.
                    query = orderAscending ? query.OrderBy(cp => string.Concat(cp.MainCurrencyAbbrv, 
                        cp.CounterCurrencyAbbrv)) : query.OrderByDescending(cp => 
                        string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv));
                    break;
            }
            
            return query
                // .OrderBy(orderingParam, orderAscending) // TODO: Make use of LinqExtensions again
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage)
                .Include(cp => cp.Source)
                .Include(cp => cp.AnalysedComponents)
                .Select(cp => new CurrencyPairViewModel
                {
                    Type = cp.CurrencyPairType,
                    MainTicker = cp.MainCurrencyAbbrv,
                    CounterTicker = cp.CounterCurrencyAbbrv,
                    SourceGuid = cp.Source.Guid.ToString(),
                    Source = new SourceViewModel
                    {
                        Abbreviation = cp.Source.Abbreviation,
                        Name = cp.Source.Name
                    },
                    AnalysedComponents = cp.AnalysedComponents
                        .Where(ac => ac.DeletedAt == null & ac.IsEnabled 
                                     && !string.IsNullOrEmpty(ac.Value))
                        .Select(ac => new AnalysedComponentViewModel
                        {
                            Guid = ac.Guid,
                            Type = ac.ComponentType,
                            UiFormatting = ac.UIFormatting,
                            Value = ac.Value,
                            IsDenominated = ac.IsDenominated
                        })
                });
        }

        public long CountByMainCurrency(string mainTicker)
        {
            if (string.IsNullOrWhiteSpace(mainTicker))
                return long.MinValue;

            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled && cp.MainCurrencyAbbrv.Equals(mainTicker,
                                 StringComparison.InvariantCultureIgnoreCase))
                .LongCount();
        }

        public ICollection<CurrencyPair> GetAllByCounterCurrency(string counterCurrencyAbbrv =
            CoreConstants.GenericCounterCurrency)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                                                  && cp.CounterCurrencyAbbrv.Equals(counterCurrencyAbbrv,
                                                      StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public ICollection<CurrencyPair> GetAllByTickerPairAbbreviation(string tickerPairAbbreviation,
            bool track = false)
        {
            if (!string.IsNullOrEmpty(tickerPairAbbreviation))
            {
                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled
                                                      && string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv)
                                                          .Equals(tickerPairAbbreviation,
                                                              StringComparison.InvariantCultureIgnoreCase));

                if (track)
                {
                    query.Include(cp => cp.Source)
                        .ThenInclude(s => s.SourceCurrencies)
                        .ThenInclude(sc => sc.Currency);
                }

                return query.ToList();
            }

            return null;
        }

        public AnalysedComponent GetRelatedAnalysedComponent(long analysedComponentId, AnalysedComponentType type,
            bool track = false)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.AnalysedComponents)
                .Where(cp => cp.AnalysedComponents.Any(ac => ac.Id.Equals(analysedComponentId)));

            if (track)
            {
                query = query
                    .Include(cp => cp.AnalysedComponents)
                    .ThenInclude(ac => ac.AnalysedHistoricItems);
            }

            return query
                .Select(cp => cp.AnalysedComponents.SingleOrDefault(ac => ac.ComponentType.Equals(type)))
                .Select(ac => new AnalysedComponent(ac, 0,
                    NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit))
                .SingleOrDefault();
        }

        public ICollection<AnalysedComponent> GetAnalysedComponents(long analysedComponentId, bool track = false)
        {
            if (analysedComponentId <= 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.AnalysedComponents)
                    // Look for the Currency pair that contains the component requested
                    .Where(cp => cp.AnalysedComponents
                        .Any(ac => ac.Id.Equals(analysedComponentId) && ac.CurrencyPairId.Equals(cp.Id)));

                if (track)
                {
                    query = query
                        .Include(cp => cp.Requests)
                        .ThenInclude(r => r.RequestComponents)
                        .Include(cp => cp.AnalysedComponents)
                        .ThenInclude(ac => ac.AnalysedHistoricItems);
                }

                return query
                    .SingleOrDefault()
                    ?.AnalysedComponents;
            }

            return null;
        }

        public ICollection<CurrencyPair> GetAll()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null)
                .ToList();
        }

        public CurrencyPair Get(long id, bool track = false, string userId = null)
        {
            if (track)
                return _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Include(cp => cp.Requests)
                    .Include(cp => cp.Source)
                    .Include(cp => cp.AnalysedComponents)
                    .SingleOrDefault(cp => cp.Id.Equals(id) && cp.DeletedAt == null);

            return _unitOfWork
                .GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(cp => cp.Id.Equals(id) && cp.DeletedAt == null);
        }

        public ICollection<DistinctCurrencyPairResponse> ListAll()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new DistinctCurrencyPairResponse()
                {
                    MainTicker = cp.MainCurrencyAbbrv,
                    CounterTicker = cp.CounterCurrencyAbbrv,
                    CurrencyPairType = cp.CurrencyPairType,
                    Id = cp.Id,
                    SourceAbbreviation = cp.Source.Abbreviation,
                    SourceName = cp.Source.Name
                })
                .DefaultIfEmpty()
                .ToList();
        }
    }
}