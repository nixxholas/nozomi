using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyTypeEvent
    {
        CurrencyType Get(long id, bool track = false);
        ICollection<CurrencyType> GetAll(int index = 0, bool track = false);
        ICollection<CurrencyType> GetAllActive(bool includeNested = false);
    }
}