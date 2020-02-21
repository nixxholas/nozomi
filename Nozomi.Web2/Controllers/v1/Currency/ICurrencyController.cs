using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Base.BCL.Responses;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Currency;

namespace Nozomi.Web2.Controllers.v1.Currency
{
    public interface ICurrencyController
    {
        IActionResult Create(CreateCurrencyViewModel vm);

        IActionResult Edit(ModifyCurrencyViewModel vm);

        IActionResult All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0,
            CurrencySortingEnum currencySortType = CurrencySortingEnum.None,
            Data.Models.Web.Analytical.AnalysedComponentType sortType = 
                Data.Models.Web.Analytical.AnalysedComponentType.Unknown, bool orderDescending = true,
            ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToTake = null, 
            ICollection<Data.Models.Web.Analytical.AnalysedComponentType> typesToDeepen = null);

        IActionResult GetCountByType(string currencyType);

        IActionResult GetPairCount(string slug);

        CurrencyViewModel Get(string slug);

        NozomiResult<ICollection<string>> ListAllSlugs();

        IActionResult List(string slug = null);

        ICollection<CurrencyViewModel> ListAll(int page = 0, int itemsPerPage = 50, 
            string currencyTypeName = null, bool orderAscending = true, 
            CurrencySortingEnum orderingParam = CurrencySortingEnum.None);

        NozomiResult<IReadOnlyDictionary<string, long>> GetSlugToIdMap();
    }
}
