using System.Collections.Generic;
using Nozomi.Base.Core.Responses;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    public interface ICurrencyController
    {
        NozomiResult<ICollection<string>> ListAll();

        NozomiResult<DetailedCurrencyResponse> Detailed(string slug);

        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string currencyType = "CRYPTO", int index = 0);

        NozomiPaginatedResult<EpochValuePair<decimal>> Historical(string slug, int index = 0, int perPage = 0);
    }
}
