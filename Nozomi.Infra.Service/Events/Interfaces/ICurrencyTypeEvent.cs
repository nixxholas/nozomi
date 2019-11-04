using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.CurrencyType;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyTypeEvent
    {
        CurrencyType Get(string guid, bool track = false);
        
        CurrencyType Get(long id, bool track = false);
        
        ICollection<CurrencyType> GetAll(int index = 0, bool track = false);

        ICollection<DistinctCurrencyTypeResponse> ListAll();
    }
}