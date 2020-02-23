using System;

namespace Nozomi.Infra.Compute.Services.Interfaces
{
    public interface IComputeService
    {
        void Modified(Guid guid, string userId = null);

        void Modified(string guid, string userId = null);
    }
}