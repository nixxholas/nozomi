using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Compute.Events.Interfaces
{
    public interface IComputeExpressionEvent
    {
        ComputeExpression Get(Guid guid, bool includeParent = false, bool ensureNotDeletedOrDisabled = true);
        
        ComputeExpression Get(string guid, bool includeParent = false, bool ensureNotDeletedOrDisabled = true);
        
        IEnumerable<ComputeExpression> GetByParent(Guid parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true);

        IEnumerable<ComputeExpression> GetByParent(string parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true);

        ComputeExpression GetMostOutdated(bool includeChildren = false, bool ensureNotDeletedOrDisabled = true);
    }
}