using System.Collections.Generic;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Infra.SignalR.Providers.Interfaces
{
    public interface IComponentMonitor
    {
        ICollection<ComponentViewModel> GetComponents();
    }
}