using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencySourceService
    {
        NozomiResult<string> Create(CreateCurrencySource currencySource, long userId = 0);
        NozomiResult<string> Delete(long id, long userId = 0);
    }
}