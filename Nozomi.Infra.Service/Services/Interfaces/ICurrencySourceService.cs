using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencySourceService
    {
        NozomiResult<string> Create(CreateCurrencySource currencySource, string userId = null);

        bool EnsurePairIsCreated(string mainTicker, string counterTicker, long sourceId, 
            string userId);
        
        NozomiResult<string> Delete(long id, string userId = null);
    }
}