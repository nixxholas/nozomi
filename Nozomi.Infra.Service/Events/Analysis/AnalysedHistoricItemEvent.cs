using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;

namespace Nozomi.Service.Events.Analysis
{
    public class AnalysedHistoricItemEvent : BaseEvent<AnalysedHistoricItemEvent, NozomiDbContext>, 
        IAnalysedHistoricItemEvent
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        
        public AnalysedHistoricItemEvent(ILogger<AnalysedHistoricItemEvent> logger, 
            IAnalysedComponentEvent analysedComponentEvent, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
            _analysedComponentEvent = analysedComponentEvent;
        }

        public AnalysedHistoricItem Latest(long analysedComponentId)
        {
            return _unitOfWork.GetRepository<AnalysedHistoricItem>()
                .GetQueryable()
                .AsNoTracking()
                .OrderByDescending(ahi => ahi.HistoricDateTime)
                .FirstOrDefault(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId));
        }

        public long Count(long analysedComponentId)
        {
            return _unitOfWork.GetRepository<AnalysedHistoricItem>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ahi => ahi.AnalysedComponentId.Equals(analysedComponentId) &&
                              ahi.DeletedAt == null && ahi.IsEnabled)
                .LongCount();
        }

        public ICollection<AnalysedHistoricItem> GetAll(long analysedComponentId, TimeSpan since, int page = 0)
        {
            if (// null check 
                analysedComponentId <= 0 || page < 0) return new List<AnalysedHistoricItem>();

            return _unitOfWork.GetRepository<AnalysedHistoricItem>()
                .GetQueryable()
                .AsNoTracking()
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
                var query = _unitOfWork.GetRepository<AnalysedHistoricItem>()
                    .GetQueryable()
                    .AsNoTracking()
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

        /// <summary>
        /// Obtains the Historic item count that is related to the AnalysedComponent in question
        /// </summary>
        /// <param name="analysedComponentId"></param>
        /// <param name="predicate"></param>
        /// <param name="deepTrack"></param>
        /// <returns></returns>
        public long GetRelevantComponentQueryCount(long analysedComponentId, 
            Expression<Func<AnalysedHistoricItem, bool>> predicate, bool deepTrack = false)
        {
            if (analysedComponentId > 0 && predicate != null)
            {
                // Obtain the base component to map
                var correlator = _analysedComponentEvent.Get(analysedComponentId);
                
                if (correlator == null)
                    _logger.LogWarning($"[{EventName}] GetRelevantComponentQueryCount: No correlator for " +
                                       $"analysed component {analysedComponentId}");
                
//                // Obtain all correlated analysed components
//                var correlations = _analysedComponentEvent.GetAllByCorrelation(analysedComponentId);
//                var correlationIds = correlations.Select(ac => ac.Id).ToList();
//                
//                // Make sure there are correlated analysed components
//                if (!correlations.Any())
//                    _logger.LogWarning($"[{EventName}] GetRelevantComponentQueryCount: No correlations for " +
//                                       $"analysed component {analysedComponentId}");
                
                //Debug.Assert(correlator != null, nameof(correlator) + " != null");
                
                // Obtain all Historic items for the correlated components.
                var query = _unitOfWork.GetRepository<AnalysedHistoricItem>()
                    .GetQueryable()
                    .Include(ahi => ahi.AnalysedComponent)
                    .Where(ahi => 
                        // Currency-based check
                        (correlator.CurrencyId != null && correlator.CurrencyId.Equals(ahi.AnalysedComponent.CurrencyId))
                        // CurrencyPair-based check
                        || (correlator.CurrencyPairId != null && correlator.CurrencyPairId.Equals(ahi.AnalysedComponent.CurrencyPairId))
                        // Currency type-based check
                        || (correlator.CurrencyTypeId != null && correlator.CurrencyTypeId.Equals(ahi.AnalysedComponent.CurrencyTypeId))
                    )
                    .AsQueryable();

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
                        .ThenInclude(ac => ac.CurrencyType)
                        .AsQueryable();
                }
                
                #if DEBUG
                var testQuery = query.ToList();
                var testResult = query
                    .Where(predicate)
                    .ToList();

                var currAc = _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .SingleOrDefault(ac => ac.Id.Equals(analysedComponentId));
                
                if ((!testResult.Any() || testResult.Count == 0) && (currAc?.CurrencyId != null && currAc.CurrencyId > 0
                                                                       && currAc.ComponentType.Equals(AnalysedComponentType.HourlyAveragePrice)))
                    Console.WriteLine($"[{EventName}] GetRelevantComponentQueryCount: BAD!");
                #endif

                return query
                    .Where(predicate)
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
                var query = _unitOfWork.GetRepository<AnalysedHistoricItem>()
                    .GetQueryable()
                    .AsNoTracking()
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
    }
}