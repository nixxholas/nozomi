using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;

namespace Nozomi.Ticker.Areas.v1.Currency
{
    public interface ICurrencyController
    {
        ActionResult<NozomiResult<string>> Create(CreateCurrency createCurrency);

        NozomiResult<string> Update(UpdateCurrency updateCurrency);
    }
}