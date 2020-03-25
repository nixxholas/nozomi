using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data.ViewModels.Component;
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
        private IHubContext<ComponentHub> _hub;
        
        public ComponentHubService(IHubContext<ComponentHub> hub, IComponentEvent componentEvent)
        {
            _hub = hub;
            _componentEvent = componentEvent;
        }

        public IEnumerable<ComponentViewModel> All()
        {
            return _componentEvent.ViewAll();
        }
    }
}