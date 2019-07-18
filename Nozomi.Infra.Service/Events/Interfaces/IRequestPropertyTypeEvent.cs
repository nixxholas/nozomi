using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface IRequestPropertyTypeEvent
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}