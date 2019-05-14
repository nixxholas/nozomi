using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyEvent
    {
        Currency Get(long id, bool track = false);
        
        AbbrvUniqueCurrencyResponse GetCurrencyByAbbreviation(string abbreviation);
        
        ICollection<Currency> GetCurrencyByAbbreviation(string abbreviation, bool track = false);
        
        Currency GetCurrencyByAbbreviation(string abbreviation, long currencySourceId, bool track = false);
        
        /// <summary>
        /// Provides the caller the total amount of currency currently circulating
        /// in the market.
        /// </summary>
        /// <param name="analysedComponent">The AnalysedComponent used to query the result.</param>
        /// <returns>Circulating supply of the currency in question.</returns>
        decimal GetCirculatingSupply(AnalysedComponent analysedComponent);

        ICollection<Currency> GetAll(bool includeNested = false);
        ICollection<Currency> GetAllNonDeleted(bool includeNested = false);

        /// <summary>
        /// Provides the requestor detailed currency data
        /// </summary>
        /// <param name="currencyTypeId"></param>
        /// <returns></returns>
        ICollection<DetailedCurrencyResponse> GetAllDetailed(string typeShortForm = "CRYPTO", int daysOfData = 1);

        /// <summary>
        /// Enables to caller to obtained a detailed about regarding a currency,
        /// including it's historical data, whichever declared/asked for.
        /// </summary>
        /// <param name="currencyId">The unique identifier of the currency</param>
        /// <param name="componentTypes">The components that the caller wants to obtain historical
        /// data about.</param>
        /// <returns></returns>
        DetailedCurrencyResponse GetDetailedById(long currencyId, ICollection<ComponentType> componentTypes);
        
        DetailedCurrencyResponse GetDetailedByAbbreviation(string abbreviation, ICollection<ComponentType> componentTypes);
         
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