using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Web.Controllers.APIs.v1.Currency
{
    public interface ICurrencyController
    {
        NozomiResult<DetailedCurrencyResponse> Detailed(string slug);

        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string currencyType = "CRYPTO", int index = 0);
    }
}
