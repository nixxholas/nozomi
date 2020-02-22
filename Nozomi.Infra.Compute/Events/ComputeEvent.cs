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
                .AsTracking()
                .OrderByDescending(c => c.ModifiedAt)
                .Include(c => c.Values)
                .Where(c => !c.IsFailing); // Hardcoded limit for now

            if (includeChildren)
                query = query.Include(c => c.Expressions)
                    .Include(c => c.ChildComputes)
                    .ThenInclude(cc => cc.ChildCompute)
                    .Include(c => c.ParentComputes)
                    .ThenInclude(c => c.ParentCompute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            var mostOutdated = query.Take(10).AsEnumerable()
                .FirstOrDefault(c => c.Values
                    .OrderByDescending(v => v.CreatedAt)
                    .FirstOrDefault()?.CreatedAt.AddMilliseconds(c.Delay) <= DateTime.UtcNow);
            
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