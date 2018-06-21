
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data.HubModels.Interfaces;

namespace Nozomi.Service.Hubs
{
    public class TickerHub : Hub, ITickerHubClient
    {
        public TickerHub()
        {
            
        }
        
        // We can use this to return a payload
        public async Task ReturnPayload(string payload)
        {
            await Clients.Caller.SendCoreAsync("", "");
        }
    }
}