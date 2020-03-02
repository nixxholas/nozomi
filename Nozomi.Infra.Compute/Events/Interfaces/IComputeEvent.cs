using System;
using System.Collections.Generic;

namespace Nozomi.Infra.Compute.Events.Interfaces
{
    public interface IComputeEvent
    {
        Data.Models.Web.Compute Get(Guid guid, bool includeChildren = false, bool ensureNotDeletedOrDisabled = true);
        
        Data.Models.Web.Compute Get(string guid, bool includeChildren = false, bool ensureNotDeletedOrDisabled = true);
        
        IEnumerable<Data.Models.Web.Compute> GetByParent(Guid parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true);

        IEnumerable<Data.Models.Web.Compute> GetByParent(string parentGuid, bool includeChildren = false, 
            bool ensureNotDeletedOrDisabled = true);

        Data.Models.Web.Compute GetMostOutdated(bool includeChildren = false, bool ensureNotDeletedOrDisabled = true);
    }
}