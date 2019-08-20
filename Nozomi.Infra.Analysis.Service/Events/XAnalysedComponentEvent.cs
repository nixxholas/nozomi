using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Analysis.Service.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events
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
                                                      && (ac.ModifiedAt.Add(TimeSpan.FromMilliseconds(ac.Delay)) <= DateTime.UtcNow
                                                          // Always give null ACs a chance
                                                          || string.IsNullOrEmpty(ac.Value))
                                                      && !acsToFilter.Contains(ac.Id))
                    // Order by ascending to the last modified time in addition to its delay
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
                             && (ac.ModifiedAt.Add(TimeSpan.FromMilliseconds(ac.Delay)) <= DateTime.UtcNow
                                 // Always give null ACs a chance
                                 || string.IsNullOrEmpty(ac.Value)))
                // Order by ascending to the last modified time in addition to its delay
                .OrderBy(ac => ac.ModifiedAt)
                // Take those not failing yet first
                .ThenBy(ac => ac.IsFailing)
                // Take in those null ones first
                // TODO: Find out if this is efficient
                //.ThenBy(ac => string.IsNullOrEmpty(ac.Value))
                .FirstOrDefault();
        }

        public IEnumerable<AnalysedComponent> GetNextWorkingSet(int index = 0, bool includeNonHistoricals = false)
        {
            if (!includeNonHistoricals)
                return _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .Where(ac => ac.DeletedAt == null
                                 && ac.IsEnabled
                                 && ac.ModifiedAt <= DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(ac.Delay))
                                 && !ac.StoreHistoricals)
                    .OrderBy(ac => ac.Id)
                    .ThenBy(ac => ac.ModifiedAt)
                    .ThenByDescending(ac => ac.IsFailing)
                    .Skip(index * 100)
                    .Take(100);
            
            // Got in, let's grab em.
            return _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .Where(ac => ac.DeletedAt == null
                             && ac.IsEnabled
                             && ac.ModifiedAt <= DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(ac.Delay)))
                .OrderBy(ac => ac.Id)
                .ThenBy(ac => ac.ModifiedAt)
                .ThenByDescending(ac => ac.IsFailing)
                .Skip(index * 100)
                .Take(100);
        }
    }
}