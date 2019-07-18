using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface IRequestTypeEvent
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}