using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeValueEvent : BaseEvent<ComputeValueEvent, NozomiComputeDbContext>, IComputeValueEvent
    {
        public ComputeValueEvent(ILogger<ComputeValueEvent> logger, NozomiComputeDbContext context) 
            : base(logger, context)
        {
        }

        public bool IsOutdated(string computeGuid, bool ignoreFailing = true)
        {
            if (Guid.TryParse(computeGuid, out var parsedGuid))
            {
                return _context.ComputeValues.AsNoTracking()
                    // Ensure it tallies and is active
                    .Where(cv => cv.DeletedAt == null && cv.IsEnabled && cv.Guid.Equals(parsedGuid))
                    .Include(cv => cv.Compute)
                    // Ensure we order by the failure rate first
                    .OrderBy(cv => cv.Compute.FailCount)
                    // Always check against the most recent
                    .ThenByDescending(cv => cv.CreatedAt)
                    .Take(1)
                    .AsEnumerable()
                    .Select(cv => cv.CreatedAt.AddMilliseconds(cv.Compute.Delay))
                    .FirstOrDefault() <= DateTime.UtcNow;
            }

            return false;
        }

        public bool IsOutdated(Guid computeGuid, bool ignoreFailing = true)
        {
            return _context.ComputeValues.AsNoTracking()
                // Ensure it tallies and is active
                .Where(cv => cv.DeletedAt == null && cv.IsEnabled && cv.Guid.Equals(computeGuid))
                .Include(cv => cv.Compute)
                // Ensure we order by the failure rate first
                .OrderBy(cv => cv.Compute.FailCount)
                // Always check against the most recent
                .ThenByDescending(cv => cv.CreatedAt)
                .Take(10)
                .AsEnumerable()
                .Select(cv => cv.CreatedAt.AddMilliseconds(cv.Compute.Delay))
                .FirstOrDefault() <= DateTime.UtcNow;
        }

        public ComputeValue GetLastItem(string computeGuid)
        {
            if (Guid.TryParse(computeGuid, out var parsedGuid))
            {
                return _context.ComputeValues.AsNoTracking()
                    .OrderByDescending(v => v.CreatedAt)
                    .FirstOrDefault(v => v.ComputeGuid.Equals(parsedGuid));
            }
            
            throw new NullReferenceException($"{_eventName} GetLastItem (string): Invalid Guid.");
        }

        public ComputeValue GetLastItem(Guid computeGuid)
        {
            return _context.ComputeValues.AsNoTracking()
                .OrderByDescending(v => v.CreatedAt)
                .FirstOrDefault(v => v.ComputeGuid.Equals(computeGuid));
        }
    }
}