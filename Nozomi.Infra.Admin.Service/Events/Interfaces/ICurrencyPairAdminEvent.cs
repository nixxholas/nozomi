using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyPairAdminEvent
    {
        CurrencyPair Get(long id, bool track = false);
    }
}