using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceEvent
    {
        bool SourceExists(string abbrv);
        
        SourceResponse Get(long id);
        SourceResponse Get(string abbreviation);
        
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);
    }
}