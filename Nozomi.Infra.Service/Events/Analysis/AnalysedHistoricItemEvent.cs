using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Responses;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedHistoricItemEvent : BaseEvent<AnalysedHistoricItemEvent, NozomiDbContext>, 
        IAnalysedHistoricItemEvent
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        
        public AnalysedHistoricItemEvent(ILogger<AnalysedHistoricItemEvent> logger, 
            IAnalysedComponentEvent analysedComponentEvent, NozomiDbContext context) 
            : base(logger, context)
        {
            _analysedComponentEvent = analysedComponentEvent;
        }

        public AnalysedHistoricItem Latest(long analysedComponentId)
        {
            return _context.AnalysedHistoricItems.AsNoTracking()
                .OrderByDescending(ahi => ahi.HistoricDateTime)
                .FirstOrDefault(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId));
        }

        public long Count(long analysedComponentId)
        {
            return _context.AnalysedHistoricItems.AsNoTracking()
                .Where(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId) &&
                              ahi.DeletedAt == null && ahi.IsEnabled)
                .LongCount();
        }

        public ICollection<AnalysedHistoricItem> GetAll(long analysedComponentId, TimeSpan since, int page = 0)
        {
            if (// null check 
                analysedComponentId <= 0 || page < 0) return new List<AnalysedHistoricItem>();

            return _context.AnalysedHistoricItems.AsNoTracking()
                .Where(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId)
                              // Obtain the ahi that is older than the current time - since
                              && ahi.HistoricDateTime < DateTime.UtcNow.Subtract(since))
                .OrderByDescending(ahi => ahi.HistoricDateTime)
                // Take only the selected 50
                .Skip(page * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                .ToList();
        }

        public IEnumerable<AnalysedHistoricItem> GetAll(long analysedComponentId, bool track = false, int index = 0)
        {
            if (analysedComponentId > 0)
            {
                var query = _context.AnalysedHistoricItems.AsNoTracking()
                    .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled 
                                                        && ahi.AnalysedComponentId.Equals(analysedComponentId));

                if (track)
                {
                    query = query.Include(ahi => ahi.AnalysedComponent);
                }

                return query
                    .OrderByDescending(ahi => ahi.HistoricDateTime)
                    .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit);
            }

            throw new ArgumentOutOfRangeException("Invalid analysedComponentId.");
        }

        public IEnumerable<AnalysedHistoricItemViewModel> List(Guid guid, int page = 0, int itemsPerPage = 50)
        {
            return _context.AnalysedHistoricItems.AsNoTracking()
                .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                .Include(ahi => ahi.AnalysedComponent)
                .Where(ahi => ahi.AnalysedComponent.Guid.Equals(guid))
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage)
                .Select(ahi => new AnalysedHistoricItemViewModel
                {
                    Timestamp = ahi.HistoricDateTime,
                    Value = ahi.Value
                });
        }

        /// <summary>
        /// Obtains the Historic item count that is related to the AnalysedComponent in question
        /// </summary>
        /// <param name="analysedComponentId"></param>
        /// <param name="predicate"></param>
        /// <param name="deepTrack"></param>
        /// <returns></returns>
        public long GetRelevantComponentQueryCount(long analysedComponentId, 
            Expression<Func<AnalysedHistoricItem, bool>> predicate, 
            Func<AnalysedHistoricItem, bool> clientPredicate = null, bool deepTrack = false)
        {
            if (analysedComponentId > 0 && predicate != null)
            {
                // Obtain the base component to map
                var correlator = _analysedComponentEvent.Get(analysedComponentId);
                
                if (correlator == null)
                    _logger.LogWarning($"[{EventName}] GetRelevantComponentQueryCount: No correlator for " +
                                       $"analysed component {analysedComponentId}");
                
                // Obtain all Historic items for the correlated components.
                var query = _context.AnalysedHistoricItems.Include(ahi => ahi.AnalysedComponent)
                    .Where(ahi => 
                        // Currency-based check
                        (correlator.CurrencyId != null && correlator.CurrencyId.Equals(ahi.AnalysedComponent.CurrencyId))
                        // CurrencyPair-based check
                        || (correlator.CurrencyPairId != null && correlator.CurrencyPairId.Equals(ahi.AnalysedComponent.CurrencyPairId))
                        // Currency type-based check
                        || (correlator.CurrencyTypeId != null && correlator.CurrencyTypeId.Equals(ahi.AnalysedComponent.CurrencyTypeId))
                    );

                if (deepTrack)
                {
                    query = query
                        .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.Currency)
                        .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.CurrencyPair)
                        .ThenInclude(cp => cp.Source)
                        .ThenInclude(s => s.SourceCurrencies)
                        .ThenInclude(sc => sc.Currency)
                        .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.CurrencyType);
                }

                query = query
                    .Where(predicate);

                if (clientPredicate != null)
                    return query
                        .AsEnumerable()
                        .Where(clientPredicate)
                        .LongCount();

                return query
                    .LongCount();
            }

            return long.MinValue;
        }
        
        public ICollection<AnalysedHistoricItem> GetRelevantHistorics(long analysedComponentId, 
            Expression<Func<AnalysedHistoricItem, bool>> predicate, 
            int index = 0)
        {
            if (analysedComponentId > 0 && predicate != null)
            {
                // Obtain all correlated analysed components
                var correlations = _analysedComponentEvent.GetAllByCorrelation(analysedComponentId);
                
                // Inside?
                var query = _context.AnalysedHistoricItems.AsNoTracking()
                    .Where(ahi => correlations.Any(ac => ac.Id.Equals(ahi.AnalysedComponentId)))
                    .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.Currency)
                        .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.CurrencyPair)
                        .ThenInclude(cp => cp.Source)
                        .ThenInclude(s => s.SourceCurrencies)
                        .ThenInclude(sc => sc.Currency)
                        .Include(ahi => ahi.AnalysedComponent)
                        .ThenInclude(ac => ac.CurrencyType)
                        .AsQueryable();

                return query
                    .Where(predicate)
                    .OrderByDescending(ahi => ahi.HistoricDateTime)
                    .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                    .ToList();
            }

            return new List<AnalysedHistoricItem>();
        }

        public NozomiPaginatedResult<AnalysedHistoricItem> TraverseRelatedHistory(long analysedComponentId, 
            AnalysedComponentType componentType, int page = 0)
        {
            // Safetynet
            if (analysedComponentId > 0 && componentType > 0 && page >= 0)
            {
                var baseAc = _context.AnalysedComponents
                    .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));

                if (baseAc == null)
                {
                    _logger.LogWarning($"[{EventName}] TraverseRelatedHistory: No base AC for " +
                                       $"analysed component {analysedComponentId}");
                    
                    return null;
                }
                
                var res = new NozomiPaginatedResult<AnalysedHistoricItem>()
                {
                    ElementsPerPage = NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit
                };
                
                // Look for the right one
                // https://stackoverflow.com/questions/661028/how-can-i-divide-two-integers-to-get-a-double
                var count = decimal.Divide(_context.AnalysedHistoricItems.Include(ahi => ahi.AnalysedComponent)
                    .Where(ahi => ahi.AnalysedComponent.ComponentType.Equals(componentType)
                        && (
                            // Currency-based matching
                            (ahi.AnalysedComponent.CurrencyId != null && ahi.AnalysedComponent.CurrencyId.Equals(baseAc.CurrencyId))
                            // Currencypair-based matching
                            || (ahi.AnalysedComponent.CurrencyPairId != null && ahi.AnalysedComponent.CurrencyPairId.Equals(baseAc.CurrencyPairId))
                            // Currency type-based matching
                            || (ahi.AnalysedComponent.CurrencyTypeId != null && ahi.AnalysedComponent.CurrencyTypeId.Equals(baseAc.CurrencyTypeId))
                            ))
                    .LongCount(), res.ElementsPerPage); // 5000 is a page's maximum number of elements.
                
                // Math Safetynet
                if (count < 1 && count > 0) // Make sure there's page 1 if the value is > 0.
                    res.Pages = 1;
                else if (count > 0) // if the count is way more than 1.
                    res.Pages = (long) count;
                else
                {
                    // Wow, bad.
                    res.Data = new List<AnalysedHistoricItem>(); // Empty it.
                    return res; // Bad response.
                }

                // If the user is trying to access beyond page 1,
                if ((page > 0 && res.Pages >= page) ||
                    // Or if the user is accessing page 1
                    (page.Equals(0)))
                {
                    // Obtain the first page
                    res.Data = _context.AnalysedHistoricItems.Include(ahi => ahi.AnalysedComponent)
                        .Where(ahi => ahi.AnalysedComponent.ComponentType.Equals(componentType)
                                      && (
                                          // Currency-based matching
                                          (ahi.AnalysedComponent.CurrencyId != null &&
                                           ahi.AnalysedComponent.CurrencyId.Equals(baseAc.CurrencyId))
                                          // Currencypair-based matching
                                          || (ahi.AnalysedComponent.CurrencyPairId != null &&
                                              ahi.AnalysedComponent.CurrencyPairId.Equals(baseAc.CurrencyPairId))
                                          // Currency type-based matching
                                          || (ahi.AnalysedComponent.CurrencyTypeId != null &&
                                              ahi.AnalysedComponent.CurrencyTypeId.Equals(baseAc.CurrencyTypeId))
                                      ))
                        .Skip((int) (page * res.ElementsPerPage))
                        .Take((int) res.ElementsPerPage) // Take only ruled
                        .ToList();

                    return res;
                }

                _logger.LogWarning($"[{EventName}] TraverseRelatedHistory: Bad Request Data for " +
                                   $"analysed component {analysedComponentId}");
            }

            return null;
        }

        public NozomiPaginatedResult<AnalysedHistoricItem> GetCurrencyPriceHistory(string slug, int index = 0, int perPage = 0)
        {
            if (index >= 0 && !string.IsNullOrEmpty(slug))
            {
                var history = _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled
                                                    && c.Slug.Equals(slug,
                                                        StringComparison.InvariantCultureIgnoreCase));

                if (history.Any())
                {
                    var result = history
                        .Include(c => c.AnalysedComponents)
                        .ThenInclude(ac => ac.AnalysedHistoricItems)
                        .SelectMany(c => c.AnalysedComponents
                            .Where(ac => ac.DeletedAt == null && ac.IsEnabled
                                                              && ac.ComponentType
                                                                  .Equals(AnalysedComponentType
                                                                      .CurrentAveragePrice)))
                        .SelectMany(ac => ac.AnalysedHistoricItems
                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                            .OrderByDescending(ahi => ahi.HistoricDateTime));
                    
                    if (perPage > 0)
                        return new NozomiPaginatedResult<AnalysedHistoricItem>
                        {
                            Pages = result.LongCount() / perPage,
                            ElementsPerPage = perPage,
                            Data = result
                                .Skip(index * perPage)
                                .Take(perPage)
                                .ToList()
                        };
                    
                    return new NozomiPaginatedResult<AnalysedHistoricItem>
                    {
                        Pages = result.LongCount() / perPage,
                        ElementsPerPage = NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit,
                        Data = result
                            .Skip(index * NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .Take(NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit)
                            .ToList()
                    };
                }
            }

            return new NozomiPaginatedResult<AnalysedHistoricItem>();
        }
    }
}