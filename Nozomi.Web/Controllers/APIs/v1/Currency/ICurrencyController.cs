using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Base.Core.Responses;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    public interface ICurrencyController
    {
        IActionResult GetCountByType(string currencyType = "CRYPTO");

        NozomiResult<ICollection<string>> ListAll();

        NozomiResult<DetailedCurrencyResponse> Detailed(string slug);

        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string currencyType = "CRYPTO", int index = 0,
            int countPerIndex = 20);

        NozomiResult<IReadOnlyDictionary<string, long>> GetSlugToIdMap();

        NozomiPaginatedResult<EpochValuePair<decimal>> Historical(string slug, int index = 0, int perPage = 0);
    }
}
