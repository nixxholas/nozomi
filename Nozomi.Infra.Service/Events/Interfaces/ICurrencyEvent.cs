using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyEvent
    {
        bool Any(CreateCurrency currency);
        
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