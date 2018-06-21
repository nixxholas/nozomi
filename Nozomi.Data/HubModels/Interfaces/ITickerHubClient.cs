using System.Diagnostics;
using System.Threading.Tasks;

namespace Nozomi.Data.HubModels.Interfaces
{
    public interface ITickerHubClient
    {
        Task ReturnPayload(string payload);
    }
}