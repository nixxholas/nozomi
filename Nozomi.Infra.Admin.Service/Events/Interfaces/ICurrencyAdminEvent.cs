using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyAdminEvent
    {
        Currency GetCurrencyBySlug(string slug);

        ICollection<Currency> GetAll(bool track = false);
    }
}