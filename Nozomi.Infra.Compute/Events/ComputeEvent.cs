using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeEvent : BaseEvent<ComputeEvent, NozomiComputeDbContext>, IComputeEvent
    {
        private readonly IComputeValueEvent _computeValueEvent;
        
        public ComputeEvent(ILogger<ComputeEvent> logger, NozomiComputeDbContext context,
            IComputeValueEvent computeValueEvent) 
            : base(logger, context)
        {
            _computeValueEvent = computeValueEvent;
        }

        public Data.Models.Web.Compute Get(Guid guid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            var query = _context.Computes.Where(c => c.Guid.Equals(guid));

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
            
            var query = _context.Computes.Where(c => c.Guid.Equals(parsedGuid));

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
            var query = _context.SubComputes.Include(sc => sc.ParentCompute)
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
            
            var query = _context.SubComputes
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
            var query = _context.Computes.AsNoTracking()
                .Include(c => c.ChildComputes)
                .ThenInclude(cc => cc.ChildCompute)
                .ThenInclude(cc => cc.Values)
                .Include(c => c.Values)
                .OrderBy(c => c.ModifiedAt) // Order by the last modified/checked time
                .ThenBy(c => c.FailCount <= 0) // Ensure we prioritize non-failing computes
                // Filter by computes with no children or computes that have children with values.
                .Where(c => !c.ChildComputes.Any() || // Ensure there are no child computes
                            // Or there are NO child computes without values 
                            c.ChildComputes.Any(cc => cc.ChildCompute.Values
                                .Any(v => v.DeletedAt == null && v.IsEnabled)));
            
            #if DEBUG
            var initialQueryRes = query.ToList();
            #endif

            if (includeChildren)
                query = query.Include(c => c.Expressions)
                    .Include(c => c.ParentComputes)
                    .ThenInclude(c => c.ParentCompute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);
            
            #if DEBUG
            var outdatedOrderedList = query
                .OrderBy(c => c.ModifiedAt)
                .ThenByDescending(c => c.FailCount <= 0)
                .Take(10)
                .AsEnumerable()
                // Order by the computes that have no values and are not failing
                .OrderByDescending(c => !c.Values.Any() && c.FailCount <= 0)
                // Then by those that are outdated and not failing
                .ThenByDescending(c => c.Values
                                           // Ensure that there is a value was computed and is now outdated
                                           .Any(v => v.CreatedAt.AddMilliseconds(c.Delay) 
                                                     > DateTime.UtcNow) 
                                       && c.FailCount <= 0)
                .ToList();
            #endif

            // Obtain the most outdated property
            var mostOutdated = query
                .OrderBy(c => c.ModifiedAt)
                .ThenByDescending(c => c.FailCount <= 0)
                .Take(10)
                .AsEnumerable()
                // Order by the computes that have no values and are not failing
                .OrderByDescending(c => !c.Values.Any() && c.FailCount <= 0)
                // Then by those that are outdated and not failing
                .ThenByDescending(c => c.Values
                                           // Ensure that there is a value was computed and is now outdated
                                           .Any(v => v.CreatedAt.AddMilliseconds(c.Delay) 
                                                     > DateTime.UtcNow) 
                                       && c.FailCount <= 0)
                .FirstOrDefault();
            
            return mostOutdated;
        }
    }
}