using System;

namespace Nozomi.Infra.Compute.Services.Interfaces
{
    public interface IComputeService
    {
        void Modified(Guid guid, bool failed = false, string userId = null);

        void Modified(string guid, bool failed = false, string userId = null);

        void Create(Data.Models.Web.Compute compute, string userId);

        void Delete(Guid guid, string userId);
    }
}