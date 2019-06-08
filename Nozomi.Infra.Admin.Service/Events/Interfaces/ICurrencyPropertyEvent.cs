using System.Collections.Generic;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyPropertyEvent
    {
        ICollection<CurrencyProperty> GetAll(int index = 0, bool track = false);

        CurrencyProperty Get(long id, bool track = false);

        ICollection<CurrencyProperty> GetAllByCurrency(long currencyId, bool track = false);
    }
}