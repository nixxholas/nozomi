using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface ICurrencyPropertyService
    {
        long Create(CurrencyProperty currencyProperty, long userId = 0);

        bool Update(CurrencyProperty currencyProperty, long userId = 0);

        bool Delete(long currencyPropertyId, bool hardDelete = false, long userId = 0);
    }
}