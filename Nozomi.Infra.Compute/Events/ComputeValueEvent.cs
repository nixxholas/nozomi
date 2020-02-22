using System;
using Microsoft.Extensions.Logging;
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
            throw new NotImplementedException();
        }

        public bool IsOutdated(Guid computeGuid)
        {
            throw new NotImplementedException();
        }
    }
}