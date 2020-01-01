using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyTypeEvent
    {
        IEnumerable<CurrencyTypeViewModel> All();
        
        CurrencyType Get(string guid, bool track = false);
        
        CurrencyType Get(long id, bool track = false);
        
        ICollection<CurrencyType> GetAll(int index = 0, bool track = false);

        ICollection<CurrencyTypeViewModel> ListAll(int page = 0, int itemsPerPage = 50, bool orderAscending = true, 
            string orderingParam = "TypeShortForm");
    }
}