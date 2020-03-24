using Microsoft.AspNetCore.SignalR;
using Nozomi.Infra.SignalR.Hubs.Services.Interfaces;

namespace Nozomi.Infra.SignalR.Hubs.Services
{
    /// <summary>
    /// Component SignalR Egress
    /// </summary>
    public class ComponentHubService : IComponentHubService
    {
        public ComponentHubService(IHubContext<ComponentHub> hub)
        {
            Hub = hub;
        }

        private IHubContext<ComponentHub> Hub { get; set; }
    }
}