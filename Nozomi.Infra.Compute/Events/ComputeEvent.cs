using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Compute.Events
{
    public class ComputeEvent : BaseEvent<ComputeEvent, NozomiDbContext>, IComputeEvent
    {
        public ComputeEvent(ILogger<ComputeEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public Data.Models.Web.Compute Get(Guid guid, bool includeChildren = false)
        {
            throw new NotImplementedException();
        }

        public Data.Models.Web.Compute Get(string guid, bool includeChildren = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Data.Models.Web.Compute> GetByParent(Guid parentGuid, bool includeChildren = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Data.Models.Web.Compute> GetByParent(string parentGuid, bool includeChildren = false)
        {
            throw new NotImplementedException();
        }
    }
}