using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Categorisation;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
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

        Item Get(long id, bool track = false);
        
        Item GetCurrencyByAbbreviation(string abbreviation, bool track = false);

        Item GetBySlug(string slug);
        
        /// <summary>
        /// Provides the caller the total amount of currency currently circulating
        /// in the market.
        /// </summary>
        /// <param name="analysedComponent">The AnalysedComponent used to query the result.</param>
        /// <returns>Circulating supply of the currency in question.</returns>
        decimal GetCirculatingSupply(AnalysedComponent analysedComponent);

        long Count(bool ignoreDeleted = false, bool ignoreDisabled = false);

        long GetCountByType(string typeShortForm);
        
        ICollection<Item> GetAll(bool includeNested = false);
        ICollection<Item> GetAllNonDeleted(bool includeNested = false);
        
        ICollection<CurrencyDTO> GetAllDTO();
        
        bool Any(CreateCurrency currency);
        
        IEnumerable<Item> GetAllActive(bool includeNested = false);
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