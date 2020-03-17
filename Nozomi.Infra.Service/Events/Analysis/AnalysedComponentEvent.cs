using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Extensions;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedComponentEvent : BaseEvent<AnalysedComponentEvent, NozomiDbContext, AnalysedComponent>, 
        IAnalysedComponentEvent
    {
        public AnalysedComponentEvent(ILogger<AnalysedComponentEvent> logger, NozomiDbContext context) 
            : base(logger, context)
        {
        }

        public IEnumerable<AnalysedComponentViewModel> All(string currencySlug, string currencyPairGuid, string currencyTypeAbbrv, int index = 0,
            int itemsPerPage = NozomiServiceConstants.AnalysedComponentTakeoutLimit, string userId = null)
        {
            if (index < 0)
                index = 0;

            if (itemsPerPage > NozomiServiceConstants.AnalysedComponentTakeoutLimit || itemsPerPage < 0)
                itemsPerPage = NozomiServiceConstants.AnalysedComponentTakeoutLimit;
                
            var query = _context.AnalysedComponents.AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(ac => ac.CreatedById.Equals(userId));
            
            if (!string.IsNullOrEmpty(currencySlug) || !string.IsNullOrWhiteSpace(currencySlug))
            {
                return query
                    .Where(ac => ac.DeletedAt == null && ac.CurrencyId != null)
                    .Include(ac => ac.Currency)
                    .Where(ac => ac.Currency.Slug.Equals(currencySlug))
                    .Skip(index * itemsPerPage)
                    .Take(itemsPerPage)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Value = ac.Value,
                        CurrencySlug = currencySlug,
                        IsEnabled = ac.IsEnabled,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        UiFormatting = ac.UIFormatting,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals
                    });
            }

            if (Guid.TryParse(currencyPairGuid, out var cpGuid))
            {
                return query
                    .Where(ac => ac.DeletedAt == null && ac.CurrencyPairId != null)
                    .Include(ac => ac.CurrencyPair)
                    .Where(ac => ac.CurrencyPair.Guid.Equals(cpGuid))
                    .Skip(index * itemsPerPage)
                    .Take(itemsPerPage)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Value = ac.Value,
                        CurrencyPairGuid = ac.CurrencyPair.Guid.ToString(),
                        IsEnabled = ac.IsEnabled,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        UiFormatting = ac.UIFormatting,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals
                    });
            } 
            
            if (!string.IsNullOrEmpty(currencyTypeAbbrv) || !string.IsNullOrWhiteSpace(currencyTypeAbbrv))
            {
                return query
                    .Where(ac => ac.DeletedAt == null && ac.CurrencyTypeId != null)
                    .Include(ac => ac.CurrencyType)
                    .Where(ac => ac.CurrencyType.TypeShortForm.Equals(currencyTypeAbbrv))
                    .Skip(index * itemsPerPage)
                    .Take(itemsPerPage)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Value = ac.Value,
                        CurrencyPairGuid = ac.CurrencyType.Guid.ToString(),
                        IsEnabled = ac.IsEnabled,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        UiFormatting = ac.UIFormatting,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals
                    });
            }

            throw new ArgumentOutOfRangeException("You need to have a unique identifier to obtain results.");
        }

        public bool Exists(AnalysedComponentType type, long currencyId = 0, string currencySlug = null, 
            string currencyPairGuid = null, string currencyTypeShortForm = null)
        {
            if (currencyId > 0)
                return _context.AnalysedComponents.AsNoTracking()
                    .Any(ac => ac.DeletedAt == null && ac.IsEnabled 
                                                    && ac.CurrencyId.Equals(currencyId)
                                                    && ac.ComponentType.Equals(type));
            
            if (!string.IsNullOrEmpty(currencySlug))
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                    .Include(ac => ac.Currency)
                    .Any(ac => ac.DeletedAt == null && ac.IsEnabled 
                                                    && ac.Currency.Slug.Equals(currencySlug)
                                                    && ac.ComponentType.Equals(type));
            
            if (Guid.TryParse(currencyPairGuid, out var cpGuid))
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled 
                                 && ac.CurrencyPairId != null)
                    .Include(ac => ac.CurrencyPair)
                    .Any(ac => ac.DeletedAt == null && ac.IsEnabled 
                                                    && ac.CurrencyPair.Guid.Equals(cpGuid)
                                                    && ac.ComponentType.Equals(type));
            
            if (!string.IsNullOrEmpty(currencyTypeShortForm))
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled
                                 && ac.CurrencyTypeId != null)
                    .Include(ac => ac.CurrencyType)
                    .Any(ac =>  ac.DeletedAt == null && ac.IsEnabled 
                                                     && ac.CurrencyType.TypeShortForm.Equals(currencyTypeShortForm)
                                                     && ac.ComponentType.Equals(type));
            
            throw new ArgumentOutOfRangeException("Foreign key out of range for logic.");
        }

        public AnalysedComponent Get(long id, bool track = false, int index = 0)
        {
            var query = _context.AnalysedComponents.AsNoTracking()
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

        public AnalysedComponent Get(string guid)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                return _context.AnalysedComponents.AsNoTracking()
                    .SingleOrDefault(ac => ac.Guid.Equals(parsedGuid));
            }
            
            throw new NullReferenceException($"{_eventName} Get (string): Invalid guid.");
        }

        public UpdateAnalysedComponentViewModel Get(Guid guid, string userId = null)
        {
            var query = _context.AnalysedComponents.AsNoTracking()
                .Where(ac => ac.Guid.Equals(guid));

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(ac => ac.CreatedById.Equals(userId));
            
            return query
                .Include(ac => ac.Currency)
                .Include(ac => ac.CurrencyPair)
                .Include(ac => ac.CurrencyType)
                .Select(ac => new UpdateAnalysedComponentViewModel
                {
                    Guid = guid,
                    Type = ac.ComponentType,
                    Delay = ac.Delay,
                    UiFormatting = ac.UIFormatting,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    StoreHistoricals = ac.StoreHistoricals,
                    IsEnabled = ac.IsEnabled,
                    CurrencySlug = ac.CurrencyId != null ? ac.Currency.Slug : null,
                    CurrencyPairGuid = ac.CurrencyPairId != null ? ac.CurrencyPair.Guid.ToString() : null,
                    CurrencyTypeShortForm = ac.CurrencyTypeId != null ? ac.CurrencyType.TypeShortForm : null,
                })
                .FirstOrDefault();
        }

        public IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false, int index = 0)
        {
            var query = _context.AnalysedComponents.AsNoTracking();

            if (filter)
                query = query.Where(ac => ac.IsEnabled && ac.DeletedAt == null
                                               &&  (ac.Delay == 0 || 
                                                    DateTime.UtcNow > ac.ModifiedAt.AddMilliseconds(ac.Delay)
                                                        .ToUniversalTime()));

            if (track)
                return query
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Include(ac => ac.Currency)
                    .Include(ac => ac.CurrencyPair)
                    .Include(ac => ac.CurrencyType)
                    .Select(ac => new AnalysedComponent(ac, index,
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit));

            return query;
        }

        public IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false)
        {
            var query = _context.AnalysedComponents
                .OrderBy(ac => ac.Id).AsNoTracking();

            if (filter)
                query = query.Where(ac => ac.IsEnabled && ac.DeletedAt == null
                                  &&  (ac.Delay == 0 || 
                                       DateTime.UtcNow > ac.ModifiedAt.AddMilliseconds(ac.Delay).ToUniversalTime()));

            if (track)
                query = query
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Include(ac => ac.Currency)
                    .Include(ac => ac.CurrencyPair)
                    .Include(ac => ac.CurrencyType);

            return query
                .OrderBy(ac => ac.Id)
                .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit);
        }

        public ICollection<AnalysedComponent> GetAllCurrencyTypeAnalysedComponents(int index = 0, bool filter = false, bool track = false)
        {
            var query = _context.CurrencyTypes.AsNoTracking();

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
            var qCurrency = _context.Currencies.AsNoTracking()
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
            var cPairs = _context.CurrencyPairs.AsNoTracking()
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                // Make sure the source has such currency
                .Where(cp => cp.Source.SourceCurrencies.Any(sc => sc.CurrencyId.Equals(currencyId)
                                                                  // And that the main currency abbreviation matches
                                                                  // the currency's abbreviation
                                                                  && sc.Currency.Abbreviation.Equals(cp.MainTicker)))
                .Include(cp => cp.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems);

            return cPairs
                .Where(predicate)
                .LongCount();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false, int index = 0,
            long ago = long.MinValue)
        {
            if (currencyTypeId > 0)
            {
                var components = _context.AnalysedComponents
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
                var components = _context.Currencies.Where(c => c.CurrencyTypeId.Equals(currencyTypeId))
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
            Expression<Func<AnalysedComponent, bool>> predicate = null, 
            Func<AnalysedComponent, bool> clientPredicate = null, int index = 0, bool track = false)
        {
            var aComp = _context.AnalysedComponents
                .AsNoTracking()
                .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

            if (aComp == null) return new List<AnalysedComponent>();

            var query = _context.AnalysedComponents
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
                query = query.Where(predicate);
            
            if (clientPredicate != null)
                return query
                    .AsEnumerable()
                    .Where(clientPredicate)
                    .ToList();
            
            return query
                .ToList();
        }

        public ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false, int index = 0)
        {
            var query = _context.CurrencyPairs.AsNoTracking()
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
            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .Include(c => c.AnalysedComponents)
                .FirstOrDefault(c => c.AnalysedComponents
                    .Any(ac => ac.Id.Equals(analysedComponent.Id)))
                ?.Abbreviation;
        }

        public AnalysedComponent Pop(Guid guid)
        {
            return _context.AnalysedComponents
                .AsTracking()
                .SingleOrDefault(ac => ac.DeletedAt == null && ac.Guid.Equals(guid));
        }

        public AnalysedComponentViewModel View(Guid guid, int index = 0, string userId = null)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("Index for historicals is out of range.");

            var query = _context.AnalysedComponents.AsNoTracking()
                .Where(ac => ac.Guid.Equals(guid) && ac.DeletedAt == null && ac.IsEnabled);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(ac => ac.CreatedById.Equals(userId));

            return query
                .Include(ac => ac.Currency)
                .Include(ac => ac.CurrencyPair)
                .Include(ac => ac.CurrencyType)
                .Include(ac => ac.AnalysedHistoricItems)
                .Select(ac => new AnalysedComponentViewModel
                {
                    Guid = ac.Guid,
                    Type = ac.ComponentType,
                    Delay = ac.Delay,
                    IsDenominated = ac.IsDenominated,
                    StoreHistoricals = ac.StoreHistoricals,
                    UiFormatting = ac.UIFormatting,
                    Value = ac.Value,
                    IsEnabled = ac.IsEnabled,
                    CurrencySlug = ac.Currency != null ? ac.Currency.Slug : string.Empty,
                    CurrencyPairGuid = ac.CurrencyPair != null ? ac.CurrencyPair.Guid.ToString() : string.Empty,
                    CurrencyTypeShortForm = ac.CurrencyType != null ? ac.CurrencyType.TypeShortForm : string.Empty,
                    History = ac.AnalysedHistoricItems
                        .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                        .OrderByDescending(ahi => ahi.HistoricDateTime)
                        .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                        .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                        .Select(ahi => new AnalysedHistoricItemViewModel
                        {
                            Timestamp = ahi.HistoricDateTime,
                            Value = ahi.Value
                        })
                })
                .SingleOrDefault();
        }

        public IQueryable<AnalysedComponent> ViewAll(int index = 0, string userId = null)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("Invalid index.");

            var query = _context.AnalysedComponents
                .OrderByDescending(ac => ac.ModifiedAt)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(ac => ac.CreatedById.Equals(userId));

            return query.Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit);
        }

        public IQueryable<AnalysedComponentViewModel> ViewAllByIdentifier(string currencySlug = null, string tickerPair = null,
            string currencyTypeAbbreviation = null, int index = 0, string userId = null)
        {
            if (!string.IsNullOrEmpty(currencySlug))
            {
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                    .Include(ac => ac.Currency)
                    .Where(ac => ac.Currency.Slug.Equals(currencySlug))
                    .OrderBy(ac => ac.CurrencyId)
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals,
                        CurrencySlug = currencySlug,
                        UiFormatting = ac.UIFormatting,
                        Value = ac.Value,
                        IsEnabled = true,
                        History = ac.AnalysedHistoricItems
                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                            .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Select(ahi => new AnalysedHistoricItemViewModel
                            {
                                Timestamp = ahi.HistoricDateTime,
                                Value = ahi.Value
                            })
                    });
            } else if (!string.IsNullOrEmpty(tickerPair))
            {
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                    .Include(ac => ac.CurrencyPair)
                    .Where(ac => ac.CurrencyPairId != null)
                    .OrderBy(ac => ac.CurrencyPairId)
                    .Where(ac => (ac.CurrencyPair.MainTicker + ac.CurrencyPair.CounterTicker)
                        == tickerPair)
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals,
                        CurrencyPairGuid = ac.CurrencyPair.Guid.ToString(),
                        UiFormatting = ac.UIFormatting,
                        Value = ac.Value,
                        IsEnabled = true,
                        History = ac.AnalysedHistoricItems
                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                            .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Select(ahi => new AnalysedHistoricItemViewModel
                            {
                                Timestamp = ahi.HistoricDateTime,
                                Value = ahi.Value
                            })
                    });
            } else if (!string.IsNullOrEmpty(currencyTypeAbbreviation))
            {
                return _context.AnalysedComponents.AsNoTracking()
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                    .Include(ac => ac.CurrencyType)
                    .Where(ac => ac.CurrencyTypeId != null 
                                 && ac.CurrencyType.TypeShortForm == currencyTypeAbbreviation)
                    .OrderBy(ac => ac.ModifiedAt)
                    .Include(ac => ac.AnalysedHistoricItems)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Select(ac => new AnalysedComponentViewModel
                    {
                        Guid = ac.Guid,
                        Type = ac.ComponentType,
                        Delay = ac.Delay,
                        IsDenominated = ac.IsDenominated,
                        StoreHistoricals = ac.StoreHistoricals,
                        CurrencyTypeShortForm = currencyTypeAbbreviation,
                        UiFormatting = ac.UIFormatting,
                        Value = ac.Value,
                        IsEnabled = true,
                        History = ac.AnalysedHistoricItems
                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                            .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Select(ahi => new AnalysedHistoricItemViewModel
                            {
                                Timestamp = ahi.HistoricDateTime,
                                Value = ahi.Value
                            })
                    });
            }

            throw new ArgumentNullException("Invalid constraints in parameter.");
        }
    }
}