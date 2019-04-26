using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Source;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceEvent
    {
        bool SourceExists(string abbrv);
        
        XSourceResponse Get(long id);
        XSourceResponse Get(string abbreviation);
        
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false);

        IEnumerable<Source> GetAll(bool countPairs = false, bool includeNested = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);
    }
}