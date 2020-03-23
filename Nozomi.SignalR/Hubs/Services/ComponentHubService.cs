using Microsoft.AspNetCore.SignalR;
using Nozomi.SignalR.Hubs.Services.Interfaces;

namespace Nozomi.SignalR.Hubs.Services
{
    public class ComponentHubService : IComponentHubService
    {
        public ComponentHubService(IHubContext<ComponentHub> hub)
        {
            Hub = hub;
        }

        private IHubContext<ComponentHub> Hub { get; set; }
    }
}