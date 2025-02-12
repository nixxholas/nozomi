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
    public class ComputeExpressionEvent : BaseEvent<ComputeExpressionEvent, NozomiComputeDbContext>,
        IComputeExpressionEvent
    {
        public ComputeExpressionEvent(ILogger<ComputeExpressionEvent> logger, NozomiComputeDbContext context) 
            : base(logger, context)
        {
        }

        public ComputeExpression Get(Guid guid, bool includeParent = false, bool ensureNotDeletedOrDisabled = true)
        {
            var query = _context.ComputeExpressions.AsNoTracking()
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
            
            var query = _context.ComputeExpressions.AsNoTracking()
                .Where(e => e.Guid.Equals(parsedGuid));

            if (includeParent)
                query = query.Include(e => e.Compute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query.SingleOrDefault();
        }

        public IEnumerable<ComputeExpression> GetByParent(Guid parentGuid, bool ensureNotDeletedOrDisabled = true)
        {
            var query = _context.ComputeExpressions.AsNoTracking()
                .Include(e => e.Compute)
                .Where(e => e.ComputeGuid.Equals(parentGuid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query;
        }

        public IEnumerable<ComputeExpression> GetByParent(string parentGuid, bool ensureNotDeletedOrDisabled = true)
        {
            if (!Guid.TryParse(parentGuid, out var parsedGuid))
                throw new NullReferenceException($"{_eventName} GetByParent (string): Invalid guid.");
            
            var query = _context.ComputeExpressions.AsNoTracking()
                .Include(e => e.Compute)
                .Where(e => e.ComputeGuid.Equals(parsedGuid));

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query;
        }

        public ComputeExpression GetMostOutdated(bool includeParent = false, bool ensureNotDeletedOrDisabled = true)
        {
            var query = _context.ComputeExpressions.AsNoTracking()
                .OrderBy(e => e.ModifiedAt)
                .ThenByDescending(e => e.Value)
                .Where(e => !string.IsNullOrEmpty(e.Expression));

            if (includeParent)
                query = query.Include(e => e.Compute);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled);

            return query.FirstOrDefault();
        }

        public IEnumerable<ComputeExpression> GetByAge(int chunkOut = 100, bool ensureNotDeletedOrDisabled = true)
        {
            var query = _context.ComputeExpressions.AsNoTracking()
                .OrderBy(e => e.ModifiedAt)
                .Include(e => e.Compute)
                .Take(chunkOut);

            if (ensureNotDeletedOrDisabled)
                query = query.Where(e => e.DeletedAt == null && e.IsEnabled
                                                             && !e.Compute.IsEnabled && e.Compute.DeletedAt == null);

            return query;
        }
    }
}