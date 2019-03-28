using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyTypeEvent
    {
        ICollection<CurrencyType> GetAllActive(bool includeNested = false);
    }
}