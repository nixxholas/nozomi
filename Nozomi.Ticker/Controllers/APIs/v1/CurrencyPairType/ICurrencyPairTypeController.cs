using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.CurrencyPairType
{
    public interface ICurrencyPairTypeController
    {
        NozomiResult<JsonResult> All();
    }
}