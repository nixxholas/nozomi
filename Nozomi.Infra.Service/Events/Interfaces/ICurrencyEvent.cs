using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Data.ViewModels.Source;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ICurrencyEvent
    {
        bool Exists(string slug);

        CurrencyViewModel Get(string slug);

        IEnumerable<BaseCurrencyViewModel> All(string slug = null);

        IEnumerable<CurrencyViewModel> All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0, 
            ICollection<ComponentType> typesToTake = null,
            ICollection<ComponentType> typesToDeepen = null);
        
        IEnumerable<CurrencyViewModel> All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0, 
            CurrencySortingEnum currencySortingEnum = CurrencySortingEnum.None,
            AnalysedComponentType sortType = AnalysedComponentType.Unknown, bool orderDescending = true, 
            ICollection<AnalysedComponentType> typesToTake = null, 
            ICollection<AnalysedComponentType> typesToDeepen = null);

        Currency Get(long id, bool track = false);
        
        Currency GetCurrencyByAbbreviation(string abbreviation, bool track = false);

        Currency GetBySlug(string slug);
        
        /// <summary>
        /// Provides the caller the total amount of currency currently circulating
        /// in the market.
        /// </summary>
        /// <param name="analysedComponent">The AnalysedComponent used to query the result.</param>
        /// <returns>Circulating supply of the currency in question.</returns>
        decimal GetCirculatingSupply(AnalysedComponent analysedComponent);

        long Count(bool ignoreDeleted = false, bool ignoreDisabled = false);

        long GetCountByType(string typeShortForm);
        
        ICollection<Currency> GetAll(bool includeNested = false);
        ICollection<Currency> GetAllNonDeleted(bool includeNested = false);
        
        ICollection<CurrencyDTO> GetAllDTO();

        /// <summary>
        /// Provides the requestor detailed currency data
        /// </summary>
        /// <param name="currencyTypeId"></param>
        /// <returns></returns>
        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string typeShortForm = "CRYPTO", int index = 0, 
            int countPerIndex = 20, int daysOfData = 7);

        /// <summary>
        /// Enables to caller to obtained a detailed about regarding a currency,
        /// including it's historical data, whichever declared/asked for.
        /// </summary>
        /// <param name="currencyId">The unique identifier of the currency</param>
        /// <param name="componentTypes">The components that the caller wants to obtain historical
        /// data about.</param>
        /// <returns></returns>
        DetailedCurrencyResponse GetDetailedById(long currencyId, ICollection<AnalysedComponentType> componentTypes);
        
        DetailedCurrencyResponse GetDetailedBySlug(string slug,
            ICollection<ComponentType> componentTypes, ICollection<AnalysedComponentType> analysedComponentTypes, 
            int componentTypesIndex = 0, int analysedComponentTypesIndex = 0);
         
        bool Any(CreateCurrency currency);
        
        IEnumerable<Currency> GetAllActive(bool includeNested = false);
        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false);
        IEnumerable<dynamic> GetAllActiveDistinctObsc(bool includeNested = false);

        ICollection<AnalysedComponent> GetTickerPairComponents(long currencyId, bool ensureValid = false, 
            int index = 0, bool track = false, Expression<Func<AnalysedComponent, bool>> predicate = null, 
            Func<AnalysedComponent, bool> clientPredicate = null, int historicItemIndex = 0);

        ICollection<string> ListAllSlugs();

        IEnumerable<CurrencyViewModel> ListAll(int page = 0, int itemsPerPage = 50, string currencyTypeName = null, 
            bool orderAscending = true, CurrencySortingEnum orderingParam = CurrencySortingEnum.None);

        IReadOnlyDictionary<string, long> ListAllMapped();

        long SourceCount(string slug);

        IEnumerable<SourceViewModel> ListSources(string slug, int page = 0, int itemsPerPage = 50);
    }
}