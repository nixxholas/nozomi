using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedComponentEvent : BaseEvent<AnalysedComponentEvent, NozomiDbContext, AnalysedComponent>, IAnalysedComponentEvent
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
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit))
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
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit));
            }

            return query;
        }

        public IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .OrderBy(ac => ac.Id);

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
                .OrderBy(ac => ac.Id)
                .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit);
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
                .Select(ac => new AnalysedComponent(ac, index, 
                    NozomiServiceConstants.AnalysedComponentTakeoutLimit, 
                    ahi => ahi.DeletedAt == null && ahi.IsEnabled))
                .ToList();
        }

        /// <summary>
        /// Obtains all Analysed Components relevant to the currency in question based on the generic counter currency.
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="ensureValid"></param>
        /// <param name="track"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool ensureValid = false, bool track = false,
            int index = 0)
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
                .Select(ac => new AnalysedComponent(ac, index, 
                    NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                .ToList();
        }

        public long GetTickerPairComponentsByCurrencyCount(long currencyId, Func<CurrencyPair, bool> predicate)
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
                                                                  && sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv)))
                .Include(cp => cp.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems);

            return cPairs
                .Where(predicate)
                .LongCount();
        }

        public ICollection<AnalysedComponent> GetTickerPairComponentsByCurrency(long currencyId, bool ensureValid = false, 
            int index = 0, bool track = false, Expression<Func<AnalysedComponent, bool>> predicate = null, 
            int historicItemIndex = 0)
        {
            var cPairs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                .OrderBy(cp => cp.Id)
                // Make sure the source has such currency
                .Where(cp => cp.Source != null && cp.Source.SourceCurrencies != null
                                               && cp.Source.SourceCurrencies.Any(sc => sc.CurrencyId.Equals(currencyId)
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

            if (predicate != null)
            {
                return cPairs
                    .OrderBy(cp => cp.Id)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .SelectMany(cp => cp.AnalysedComponents)
                    .Where(predicate)
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                    .ToList();
            }

            return cPairs
                .OrderBy(cp => cp.Id)
                .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .SelectMany(cp => cp.AnalysedComponents)
                .Select(ac => new AnalysedComponent(ac, index, 
                    NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false, int index = 0,
            long ago = long.MinValue)
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

                if (ago > 0)
                {
                    return components
                        .Select(ac => new AnalysedComponent(ac, index, 
                            NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                        .ToList();
                }

                return components
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                    .ToList();
            }

            return null;
        }

        public ICollection<AnalysedComponent> GetAllCurrencyComponentsByType(long currencyTypeId, bool track = false,
            int index = 0)
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
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                    .ToList();
            }

            return null;
        }

        public ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, 
            Expression<Func<AnalysedComponent, bool>> predicate = null, int index = 0, bool track = false)
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
                
                query = query
                    .Select(ac => new AnalysedComponent(ac, index, 
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit));
            }

            if (predicate != null)
                return query.Where(predicate).ToList();
            
            return query
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false, int index = 0)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.Id.Equals(currencyPairId))
                .Include(r => r.AnalysedComponents);

            if (!track)
                query.ThenInclude(ac => ac.AnalysedHistoricItems);
            
            return query
                .SelectMany(r => r.AnalysedComponents)
                .Select(ac => new AnalysedComponent(ac, index, NozomiServiceConstants.AnalysedComponentTakeoutLimit))
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
                ?.Abbreviation;
        }
    }
}