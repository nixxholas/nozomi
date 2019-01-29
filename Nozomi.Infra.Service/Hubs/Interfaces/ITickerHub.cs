using Nozomi.Preprocessing.Hubs.Enumerators;

namespace Nozomi.Service.Hubs.Interfaces
{
    public interface ITickerHub
    {
        void Register(TickerHubGroup hubGroup);

        void Unregister(TickerHubGroup hubGroup);
    }
}