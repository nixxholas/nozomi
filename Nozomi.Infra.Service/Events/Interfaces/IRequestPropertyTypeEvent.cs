using System.Collections.Generic;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestPropertyTypeEvent
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}