using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ViewModels.CurrencyType;

namespace Nozomi.Web2.Controllers.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        IActionResult All();

        ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page);

        IActionResult ListAll();
    }
}
