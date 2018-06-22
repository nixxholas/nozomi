using System.Diagnostics;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.HostedServices.RequestTypes.Interfaces
{
    public interface IHttpGetSyncingService
    {
        bool Process(Request req);
    }
}