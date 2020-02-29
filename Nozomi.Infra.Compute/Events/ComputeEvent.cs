using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Extensions;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeEvent : BaseEvent<ComputeEvent, NozomiComputeDbContext>, IComputeEvent
    {
        private readonly IComputeValueEvent _computeValueEvent;
        
        public ComputeEvent(ILogger<ComputeEvent> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public Data.Models.Web.Compute Get(Guid guid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            var query = _unitOfWork.GetRepository<Data.Models.Web.Compute>()
                .GetQueryable()
                .Where(c => c.Guid.Equals(guid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            if (includeChildren)
                query = query.Include(c => c.Expressions)
                    .Include(c => c.ParentComputes)
                    .ThenInclude(pc => pc.ParentCompute)
                    .Include(c => c.ChildComputes)
                    .ThenInclude(cc => cc.ChildCompute);

            return query.SingleOrDefault();
        }

        public Data.Models.Web.Compute Get(string guid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
            {
                _logger.LogWarning($"{_eventName} Get (string): Invalid guid.");
                return null;
            }
            
            var query = _unitOfWork.GetRepository<Data.Models.Web.Compute>()
                .GetQueryable()
                .Where(c => c.Guid.Equals(parsedGuid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            if (includeChildren)
                query = query.Include(c => c.Expressions)
                    .Include(c => c.ParentComputes)
                    .ThenInclude(pc => pc.ParentCompute)
                    .Include(c => c.ChildComputes)
                    .ThenInclude(cc => cc.ChildCompute);

            return query.SingleOrDefault();
        }

        public IEnumerable<Data.Models.Web.Compute> GetByParent(Guid parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            var query = _unitOfWork.GetRepository<SubCompute>()
                .GetQueryable()
                .Include(sc => sc.ParentCompute)
                .Where(c => c.ParentComputeGuid.Equals(parentGuid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            if (includeChildren)
                query = query.Include(sc => sc.ParentCompute)
                    .ThenInclude(pc => pc.ChildComputes)
                    .ThenInclude(cc => cc.ChildCompute)
                    .Include(sc => sc.ParentCompute)
                    .ThenInclude(pc => pc.ParentComputes)
                    .ThenInclude(pc => pc.ParentCompute);

            return query.Select(sc => sc.ParentCompute);
        }

        public IEnumerable<Data.Models.Web.Compute> GetByParent(string parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            if (!Guid.TryParse(parentGuid, out var parsedGuid))
            {
                _logger.LogWarning($"{_eventName} GetByParent (string): Invalid guid.");
                return null;
            }
            
            var query = _unitOfWork.GetRepository<SubCompute>()
                .GetQueryable()
                .Include(sc => sc.ParentCompute)
                .Where(c => c.ParentComputeGuid.Equals(parsedGuid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            if (includeChildren)
                query = query.Include(sc => sc.ParentCompute)
                    .ThenInclude(pc => pc.ChildComputes)
                    .ThenInclude(cc => cc.ChildCompute)
                    .Include(sc => sc.ParentCompute)
                    .ThenInclude(pc => pc.ParentComputes)
                    .ThenInclude(pc => pc.ParentCompute);

            return query.Select(sc => sc.ParentCompute);
        }

        public Data.Models.Web.Compute GetMostOutdated(bool includeChildren = false, bool ensureNotDeletedOrDisabled = true)
        {
            var query = _unitOfWork.GetRepository<Data.Models.Web.Compute>()
                .GetQueryable()
                .AsNoTracking()
                .Include(c => c.ChildComputes)
                .ThenInclude(cc => cc.ChildCompute)
                .ThenInclude(cc => cc.Values)
                .Include(c => c.Values)
                // Order by the computes that have a child that has a values
                // .OrderByDescending(c => c.ChildComputes
                //                             .Any(cc => cc.ChildCompute
                //                                 .Values.Any(v => v.DeletedAt == null && v.IsEnabled))
                //                         // Or computes that have no children.
                //                         || !c.ChildComputes.Any())
                .OrderBy(c => c.ModifiedAt) // Then we prioritize by the last modified/checked time
                .ThenBy(c => c.FailCount) // Ensure we prioritize non-failing computes
                // Filter by computes with no children or computes that have children with values.
                .Where(c => !c.ChildComputes.Any() || 
                            (c.ChildComputes.Any(cc => cc.ChildCompute.Values
                                .Any(v => v.CreatedAt == null && v.IsEnabled))));
            
            #if DEBUG
            var initialQueryRes = query.ToList();
            #endif

            if (includeChildren)
                query = query.Include(c => c.Expressions)
                    .Include(c => c.ParentComputes)
                    .ThenInclude(c => c.ParentCompute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            // var mostOutdated = query.Take(10).AsEnumerable()
            //     .FirstOrDefault(c => c.Values
            //         .OrderByDescending(v => v.CreatedAt)
            //         .FirstOrDefault()?.CreatedAt.AddMilliseconds(c.Delay) <= DateTime.UtcNow);
            var mostOutdated = query
                // Order by the computes that have no values and are not failing
                .OrderBy(c => !c.Values.Any() && c.FailCount.Equals(0))
                .Take(10)
                .AsEnumerable()
                // Then by those that are outdated and not failing
                .OrderByDescending(c => c.Values
                    .Any(v => v.CreatedAt.AddMilliseconds(c.Delay) > DateTime.UtcNow) 
                                        && c.FailCount.Equals(0))
                .FirstOrDefault();
            
#if DEBUG
            var mostOutdatedRes = mostOutdated;
#endif
            
            // Ensure we mark it as being updated.
            if (mostOutdated != null)
            {
                mostOutdated.ModifiedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<Data.Models.Web.Compute>().Update(mostOutdated);
                _unitOfWork.Commit();
            }

            return mostOutdated;
        }
    }
}