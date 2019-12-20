using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyPair;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairService
    {
        bool Create(CreateCurrencyPairViewModel vm, string userId = null);

        bool Update(UpdateCurrencyPairViewModel vm, string userId = null);
        
        NozomiResult<string> Create(CreateCurrencyPair createCurrencyPair, string userId = null);

        NozomiResult<string> Update(UpdateCurrencyPair updateCurrencyPair, string userId = null);

        NozomiResult<string> Delete(long currencyPairId, string userId = null, bool hardDelete = false);
        
        long[][] GetCurrencySourceMappings();
        //IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id);
        IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false);
        IEnumerable<string> GetAllCurrencyPairUrls();
    }
}
