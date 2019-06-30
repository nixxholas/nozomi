using System.Collections.Generic;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyAdminEvent
    {
        Currency GetCurrencyBySlug(string slug, bool track = false);

        ICollection<Currency> GetAll(bool track = false);
    }
}