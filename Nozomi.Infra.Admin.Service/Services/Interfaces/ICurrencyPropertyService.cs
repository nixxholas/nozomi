using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface ICurrencyPropertyService
    {
        long Create(CurrencyProperty currencyProperty, string userId = null);

        bool Update(CurrencyProperty currencyProperty, string userId = null);

        bool Delete(long currencyPropertyId, bool hardDelete = false, string userId = null);
    }
}