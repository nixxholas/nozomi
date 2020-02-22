using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeEvent : BaseEvent<ComputeEvent, NozomiComputeDbContext>, IComputeEvent
    {
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
    }
}