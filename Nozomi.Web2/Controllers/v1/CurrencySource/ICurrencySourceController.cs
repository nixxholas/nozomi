using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.CurrencySource;

namespace Nozomi.Web2.Controllers.v1.CurrencySource
{
    public interface ICurrencySourceController
    {
        IActionResult Create(CreateCurrencySourceViewModel vm);
    }
}