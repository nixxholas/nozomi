using Nozomi.Data.Models.Currency;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface ICurrencyTypeService
    {
        long Create(CurrencyType currencyType, string userId = null);

        bool Update(CurrencyType currencyType, string userId = null);

        bool Delete(long currencyTypeId, bool hardDelete = false, string userId = null);
    }
}