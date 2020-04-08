using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Categorisation;
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
        
        ItemType Get(string slug, bool track = false);
        
        ItemType Get(Guid guid, bool track = false);
        
        ItemType Get(long id, bool track = false);
        
        ICollection<ItemType> GetAll(int index = 0, bool track = false);

        ICollection<CurrencyTypeViewModel> ListAll(int page = 0, int itemsPerPage = 50, bool orderAscending = true, 
            string orderingParam = "TypeShortForm");

        ItemType Pop(Guid guid);
    }
}