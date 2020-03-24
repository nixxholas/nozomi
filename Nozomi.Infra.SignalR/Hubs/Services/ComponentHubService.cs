using Microsoft.AspNetCore.SignalR;
using Nozomi.Infra.SignalR.Hubs.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.SignalR.Hubs.Services
{
    /// <summary>
    /// Component SignalR Egress
    /// </summary>
    public class ComponentHubService : IComponentHubService
    {
        private readonly IComponentEvent _componentEvent;
        
        public ComponentHubService(IHubContext<ComponentHub> hub, IComponentEvent componentEvent)
        {
            Hub = hub;
            _componentEvent = componentEvent;
        }

        private IHubContext<ComponentHub> Hub { get; set; }
    }
}