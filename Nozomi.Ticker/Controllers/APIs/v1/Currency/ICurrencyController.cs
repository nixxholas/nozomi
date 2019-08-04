using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Ticker.Controllers.APIs.v1.Currency
{
    public interface ICurrencyController
    {
        ActionResult<NozomiResult<string>> Create(CreateCurrency createCurrency);

        NozomiResult<GeneralisedCurrencyResponse> Detailed(string abbreviation);

        ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string currencyType = "CRYPTO", int index = 0);

        NozomiResult<string> Update(UpdateCurrency updateCurrency);
    }
}