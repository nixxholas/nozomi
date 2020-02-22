using System;

namespace Nozomi.Infra.Compute.Events.Interfaces
{
    public interface IComputeValueEvent
    {
        bool IsOutdated(string computeGuid, bool ignoreFailing = true);

        bool IsOutdated(Guid computeGuid, bool ignoreFailing = true);
    }
}