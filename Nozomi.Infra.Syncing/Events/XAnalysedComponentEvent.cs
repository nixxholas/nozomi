using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Syncing.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Syncing.Events
{
    public class XAnalysedComponentEvent : BaseEvent<XAnalysedComponentEvent, NozomiDbContext>, 
        IXAnalysedComponentEvent
    {
        public XAnalysedComponentEvent(ILogger<XAnalysedComponentEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public AnalysedComponent Top(ICollection<long> acsToFilter = null)
        {
            if (acsToFilter != null)
                return _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .AsNoTracking()
                    // Enabled?
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled 
                                                      && (// Last modified time is older than the current time in conjunction with the delay
                                                          ac.ModifiedAt.Add(TimeSpan.FromMilliseconds(ac.Delay)) 
                                                          <= DateTime.UtcNow
                                                          // Always give null ACs a chance
                                                          || string.IsNullOrEmpty(ac.Value))
                                                      && !acsToFilter.Contains(ac.Id))
                    // Order by ascending to the last modified time
                    .OrderBy(ac => ac.ModifiedAt)
                    // Take those not failing yet first
                    .ThenBy(ac => ac.IsFailing)
                    // Take in those null ones first
                    // TODO: Find out if this is efficient
                    //.ThenBy(ac => string.IsNullOrEmpty(ac.Value))
                    .FirstOrDefault();
            
            return _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                // Enabled?
                .Where(ac => ac.DeletedAt == null && ac.IsEnabled 
                                                  && (// Last modified time is older than the current time in conjunction with the delay
                                                      ac.ModifiedAt.Add(TimeSpan.FromMilliseconds(ac.Delay)) 
                                                      <= DateTime.UtcNow
                                                      // Always give null ACs a chance
                                                      || string.IsNullOrEmpty(ac.Value))
                             && !acsToFilter.Contains(ac.Id))
                // Order by ascending to the last modified time
                .OrderBy(ac => ac.ModifiedAt)
                // Take those not failing yet first
                .ThenBy(ac => ac.IsFailing)
                // Take in those null ones first
                // TODO: Find out if this is efficient
                //.ThenBy(ac => string.IsNullOrEmpty(ac.Value))
                .FirstOrDefault();
        }

        public ICollection<AnalysedComponent> GetNextWorkingSet(int index = 0, bool includeNonHistoricals = false)
        {
            var currentUtc = DateTime.UtcNow;
            
            if (!includeNonHistoricals)
                return _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ac => ac.DeletedAt == null
                                 && ac.IsEnabled
                                 && (// Last modified time is older than the current time in conjunction with the delay
                                     ac.ModifiedAt < currentUtc)
                                 && ac.StoreHistoricals == includeNonHistoricals)
                    // Order by ascending to the last modified time
                    .OrderBy(ac => ac.ModifiedAt)
                    .ThenBy(ac => // Always give null ACs a chance
                       string.IsNullOrEmpty(ac.Value))
                    .ThenByDescending(ac => ac.IsFailing)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .ToList();
            
            // Got in, let's grab em.
            return _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                // Make sure LastChecked is null
                .Where(ac => // Last modified time is older than the current time in conjunction with the delay
                       ac.ModifiedAt < currentUtc)
                // Order by ascending to the last modified time
                .OrderBy(ac => ac.ModifiedAt)
                .ThenBy(ac => // Always give null ACs a chance
                   string.IsNullOrEmpty(ac.Value))
                .ThenByDescending(ac => ac.IsFailing)
                .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .ToList();
        }
    }
}