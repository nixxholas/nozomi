using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceEvent
    {
        
        
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);
    }
}