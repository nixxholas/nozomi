using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ViewModels.CurrencyType;

namespace Nozomi.Web2.Controllers.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        IActionResult All(int index = 0, int itemsPerPage = 200);

        IActionResult Create(CreateCurrencyTypeViewModel vm);

        ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page);

        IActionResult ListAll(int page = 0, int itemsPerPage = 50, bool orderAscending = true, 
            string orderingParam = "TypeShortForm");

        IActionResult Update(UpdateCurrencyTypeViewModel vm);
    }
}
