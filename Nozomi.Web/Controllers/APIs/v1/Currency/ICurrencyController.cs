using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Base.Core.Responses;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ViewModels.Currency;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    public interface ICurrencyController
    {
        IActionResult Create(CreateCurrencyViewModel vm);

        IActionResult Edit(ModifyCurrencyViewModel vm);

        IActionResult All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0,
            AnalysedComponentType sortType = AnalysedComponentType.Unknown, bool orderDescending = true,
            ICollection<AnalysedComponentType> typesToTake = null, ICollection<AnalysedComponentType> typesToDeepen = null);

        IActionResult GetCountByType(string currencyType = "CRYPTO");

        NozomiResult<ICollection<string>> ListAll();

        NozomiResult<DetailedCurrencyResponse> Detailed(string slug);

        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string currencyType = "CRYPTO", int index = 0,
            int countPerIndex = 20);

        NozomiResult<IReadOnlyDictionary<string, long>> GetSlugToIdMap();

        NozomiPaginatedResult<EpochValuePair<decimal>> Historical(string slug, int index = 0, int perPage = 0);
    }
}
