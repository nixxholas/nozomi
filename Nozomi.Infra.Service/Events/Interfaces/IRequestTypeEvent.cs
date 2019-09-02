using System.Collections.Generic;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestTypeEvent
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}