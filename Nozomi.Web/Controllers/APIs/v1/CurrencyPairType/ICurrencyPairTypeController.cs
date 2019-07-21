using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPairType
{
    public interface ICurrencyPairTypeController
    {
        NozomiResult<JsonResult> All();
    }
}