using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairService
    {
        NozomiResult<string> Create(CreateCurrencyPair createCurrencyPair, long userId = 0);

        NozomiResult<string> Update(UpdateCurrencyPair updateCurrencyPair, long userId = 0);

        NozomiResult<string> Delete(long currencyPairId, long userId = 0, bool hardDelete = false);
        
        long[][] GetCurrencySourceMappings();
        //IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id);
        IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false);
        IEnumerable<string> GetAllCurrencyPairUrls();
    }
}
