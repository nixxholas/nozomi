using Nozomi.Realtime.Infra.Service.Hubs.Enumerators;

namespace Nozomi.Realtime.Infra.Service.Hubs.Server.Interfaces
{
    public interface ITickerHubServer
    {
        /// <summary>
        /// Centralized method/function capable of traversing to
        /// the appropriate endpoint of TickerHub.
        /// </summary>
        /// <param name="hubGroup"></param>
        void BroadcastData(TickerHubGroup hubGroup);
    }
}