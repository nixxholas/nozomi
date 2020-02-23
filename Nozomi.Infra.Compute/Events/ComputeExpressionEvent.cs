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

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeExpressionEvent : BaseEvent<ComputeExpressionEvent, NozomiComputeDbContext>,
        IComputeExpressionEvent
    {
        public ComputeExpressionEvent(ILogger<ComputeExpressionEvent> logger, 
            IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComputeExpression Get(Guid guid, bool includeParent = false, bool ensureNotDeletedOrDisabled = true)
        {
            var query =  _unitOfWork.GetRepository<ComputeExpression>()
                .GetQueryable()
                .AsNoTracking()
                .Where(e => e.Guid.Equals(guid));

            if (includeParent)
                query = query.Include(e => e.Compute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query.SingleOrDefault();
        }

        public ComputeExpression Get(string guid, bool includeParent = false, bool ensureNotDeletedOrDisabled = true)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                throw new NullReferenceException($"{_eventName} Get (string): Invalid guid.");
            
            var query =  _unitOfWork.GetRepository<ComputeExpression>()
                .GetQueryable()
                .AsNoTracking()
                .Where(e => e.Guid.Equals(parsedGuid));

            if (includeParent)
                query = query.Include(e => e.Compute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query.SingleOrDefault();
        }

        public IEnumerable<ComputeExpression> GetByParent(Guid parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ComputeExpression> GetByParent(string parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true)
        {
            throw new NotImplementedException();
        }

        public ComputeExpression GetMostOutdated(bool includeChildren = false, bool ensureNotDeletedOrDisabled = true)
        {
            throw new NotImplementedException();
        }
    }
}