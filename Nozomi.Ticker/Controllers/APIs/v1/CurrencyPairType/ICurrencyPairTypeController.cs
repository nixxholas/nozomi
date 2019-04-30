using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairType
{
    public interface ICurrencyPairTypeController
    {
        NozomiResult<JsonResult> All();
    }
}