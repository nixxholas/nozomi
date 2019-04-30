using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Ticker.Areas.v1.Currency
{
    public interface ICurrencyController
    {
        ActionResult<NozomiResult<string>> Create(CreateCurrency createCurrency);

        NozomiResult<DetailedCurrencyResponse> Detailed(string abbreviation);

        NozomiResult<string> Update(UpdateCurrency updateCurrency);
    }
}