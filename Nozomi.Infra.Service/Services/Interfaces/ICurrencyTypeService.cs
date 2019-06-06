using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyTypeService
    {
        long Create(CurrencyType currencyType, long userId = 0);

        bool Delete(long currencyTypeId, bool hardDelete = false, long userId = 0);
    }
}