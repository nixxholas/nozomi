using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;

namespace Nozomi.Web2.Controllers.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        IActionResult All();

        ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page);

        IActionResult ListAll(int page = 0, int itemsPerPage = 50, bool orderAscending = true, 
            string orderingParam = "TypeShortForm");
    }
}
