using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyType;
using Nozomi.Preprocessing;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyTypeEvent
    {
        bool Exists(string typeShortForm);

        bool Exists(Guid guid);

        bool Exists(long id);
        
        IEnumerable<CurrencyTypeViewModel> All(int index = 0, int itemsPerPage = 200);
        
        CurrencyType Get(string guid, bool track = false);
        
        CurrencyType Get(long id, bool track = false);
        
        ICollection<CurrencyType> GetAll(int index = 0, bool track = false);

        ICollection<CurrencyTypeViewModel> ListAll(int page = 0, int itemsPerPage = 50, bool orderAscending = true, 
            string orderingParam = "TypeShortForm");
    }
}