using System;
using System.Collections.Generic;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Infra.SignalR.Hubs.Services.Interfaces
{
    public interface IComponentHubService
    {
        IEnumerable<ComponentViewModel> All();
    }
}