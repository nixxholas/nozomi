using System;
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
    public class ComputeValueEvent : BaseEvent<ComputeValueEvent, NozomiComputeDbContext>, IComputeValueEvent
    {
        public ComputeValueEvent(ILogger<ComputeValueEvent> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public bool IsOutdated(string computeGuid)
        {
            if (Guid.TryParse(computeGuid, out var parsedGuid))
            {
                return _unitOfWork.GetRepository<ComputeValue>()
                    .GetQueryable()
                    .AsNoTracking()
                    // Ensure it tallies and is active
                    .Where(cv => cv.DeletedAt == null && cv.IsEnabled && cv.Guid.Equals(parsedGuid))
                    .Include(cv => cv.Compute)
                    // Always check against the most recent
                    .OrderByDescending(cv => cv.CreatedAt)
                    .Take(1)
                    .AsEnumerable()
                    .Select(cv => cv.CreatedAt.AddMilliseconds(cv.Compute.Delay))
                    .FirstOrDefault() <= DateTime.UtcNow;
            }

            return false;
        }

        public bool IsOutdated(Guid computeGuid)
        {
            throw new NotImplementedException();
        }
    }
}