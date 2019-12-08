using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        NozomiResult<string> Create(CreateCurrency currency, string userId = null);
        NozomiResult<string> Update(UpdateCurrency currency, string userId = null);
        NozomiResult<string> Delete(long currencyId, bool hardDelete = false, string userId = null);
        
    }
}
