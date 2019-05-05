using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface ICurrencyAdminEvent
    {
        AbbrvUniqueCurrencyResponse GetCurrencyByAbbreviation(string abbreviation);
    }
}