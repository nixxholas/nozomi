using System;
using System.Collections.Generic;
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

        public ComputeExpression Get(Guid guid, bool includeChildren = false, bool ensureNotDeletedOrDisabled = true)
        {
            throw new NotImplementedException();
        }

        public ComputeExpression Get(string guid, bool includeChildren = false, bool ensureNotDeletedOrDisabled = true)
        {
            throw new NotImplementedException();
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