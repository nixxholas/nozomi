using System;
using System.Collections.Generic;

namespace Nozomi.Infra.Compute.Events.Interfaces
{
    public interface IComputeEvent
    {
        Data.Models.Web.Compute Get(Guid guid, bool includeChildren = false);
        
        Data.Models.Web.Compute Get(string guid, bool includeChildren = false);
        
        IEnumerable<Data.Models.Web.Compute> GetByParent(Guid parentGuid, bool includeChildren = false);

        IEnumerable<Data.Models.Web.Compute> GetByParent(string parentGuid, bool includeChildren = false);
    }
}