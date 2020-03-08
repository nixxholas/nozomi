using System.Collections.Generic;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentTypeEvent
    {
        IEnumerable<KeyValuePair<string, long>> All();
    }
}