using System.Collections.Generic;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentTypeEvent
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}