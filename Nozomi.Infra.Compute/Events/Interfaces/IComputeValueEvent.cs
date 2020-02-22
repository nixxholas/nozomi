using System;

namespace Nozomi.Infra.Compute.Events.Interfaces
{
    public interface IComputeValueEvent
    {
        bool IsOutdated(string computeGuid);

        bool IsOutdated(Guid computeGuid);
    }
}