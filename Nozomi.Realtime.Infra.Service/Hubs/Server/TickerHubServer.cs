using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Realtime.Infra.Service.Hubs.Enumerators;
using Nozomi.Realtime.Infra.Service.Hubs.Server.Interfaces;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Realtime.Infra.Service.Hubs.Server
{
    /// <summary>
    /// Server-side functions for TickerHub.
    /// </summary>
    public class TickerHubServer : BaseEvent<TickerHubServer, NozomiDbContext>, ITickerHubServer
    {
        private readonly IHubContext<TickerHub> _tickerHub;
        private readonly ITickerEvent _tickerEvent;
        
        public TickerHubServer(ILogger<TickerHubServer> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ITickerEvent tickerEvent, IHubContext<TickerHub> tickerHub) 
            : base(logger, unitOfWork)
        {
            _tickerEvent = tickerEvent;
            _tickerHub = tickerHub;
        }
        
        public async void BroadcastData(TickerHubGroup hubGroup)
        {
            switch (hubGroup)
            {
                case TickerHubGroup.Ticker:
                    await _tickerHub.Clients.Group(hubGroup.GetDescription())
                        .SendCoreAsync(hubGroup.GetDescription(), new object[] {_tickerEvent.GetAll()});
                    break;
            }
        }

    }
}