using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencySourceService
    {
        NozomiResult<string> Create(CreateCurrencySource currencySource, string userId = null);
        NozomiResult<string> Delete(long id, string userId = null);
    }
}