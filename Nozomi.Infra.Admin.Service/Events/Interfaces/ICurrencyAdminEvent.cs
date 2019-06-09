using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyAdminEvent
    {
        Currency GetCurrencyBySlug(string slug);
    }
}