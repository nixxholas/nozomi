using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ResponseModels.CurrencyType;

namespace Nozomi.Web2.Controllers.APIs.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        IActionResult All();

        ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page);

        ICollection<DistinctCurrencyTypeResponse> ListAll();
    }
}
