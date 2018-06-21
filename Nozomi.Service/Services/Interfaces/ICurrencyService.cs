using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        long Create(Currency currency, long userId = 0);
        bool Update(long userId, long currencyId, Currency currency);
        bool SoftDelete(long currencyId, long userId = 0);
        
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
