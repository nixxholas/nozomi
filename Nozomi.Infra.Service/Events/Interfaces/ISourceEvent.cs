using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ISourceEvent
    {
        bool Exists(string guid);
        
        bool AbbreviationIsUsed(string abbrv);
        
        Source GetByGuid(string guid, bool filterActive = false);

        IEnumerable<Nozomi.Data.ViewModels.Source.SourceViewModel> GetAll();

        IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false);

        //IEnumerable<Source> GetAll(bool countPairs = false, bool includeNested = false);
        
        IEnumerable<Source> GetAllNonDeleted(bool countPairs = false, bool includeNested = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);

        IEnumerable<Source> GetAllCurrencySourceOptions(IEnumerable<CurrencySource> currencySources);

        IEnumerable<Source> GetCurrencySources(string slug, int page = 0);
    }
}