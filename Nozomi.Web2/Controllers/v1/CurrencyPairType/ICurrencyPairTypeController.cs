using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.CurrencyPairType
{
    public interface ICurrencyPairTypeController
    {
        IActionResult All();
    }
}