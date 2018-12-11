using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        NozomiResult<string> Create(CreateCurrency currency, long userId = 0);
        NozomiResult<string> Update(UpdateCurrency currency, long userId = 0);
        NozomiResult<string> Delete(long currencyId, bool hardDelete = false, long userId = 0);
        
        IEnumerable<Currency> GetAllActive(bool includeNested = false);
        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);
        IEnumerable<dynamic> GetAllActiveDistinctObsc(bool includeNested = false);

        /// <summary>
        /// Gets all pairs with currency relationship.
        /// 
        /// The key of the dictionary is what the user chooses for his main option.
        /// We'll then use the currencypair object to match for counter currencies that pair
        /// up with his option.
        /// </summary>
        /// <returns>The all pairs with currency rs.</returns>
        IDictionary<long, IDictionary<long, Tuple<string, string>>> GetAllCurrencyPairings();

        /// <summary>
        /// Gets all pairs with currency relationship in conjunction with wallettypes
        /// 
        /// </summary>
        /// <returns>The all pairs with currency rs.</returns>
        IDictionary<long, IDictionary<long, long>> GetAllWalletTypeCurrencyPairings();
    }
}
