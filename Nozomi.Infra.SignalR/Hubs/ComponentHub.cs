using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Infra.SignalR.Hubs.Services;
using Nozomi.Infra.SignalR.Hubs.Services.Interfaces;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Infra.SignalR.Hubs
{
    /// <summary>
    /// Component SignalR Hub (Ingress)
    /// </summary>
    public class ComponentHub : Hub<IComponentHubService>
    {
        private readonly IComponentEvent _componentEvent;
        private readonly ComponentHubService _componentHubService;

        public ComponentHub(ComponentHubService componentHubService, IComponentEvent componentEvent)
        {
            _componentEvent = componentEvent;
            _componentHubService = componentHubService;
        }

        public IEnumerable<ComponentViewModel> All(int index = 0)
        {
            return _componentEvent.ViewAll(index);
        }
    }
}