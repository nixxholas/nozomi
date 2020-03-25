using System.Collections.Generic;
using Nozomi.Data.ViewModels.ComponentType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentTypeEvent
    {
        IEnumerable<KeyValuePair<string, long>> All(string userId = null);

        IEnumerable<ComponentTypeViewModel> ViewAll(string userId = null, int index = 0);
    }
}