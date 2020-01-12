using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponent
{
    public interface IAnalysedComponentController
    {
        IActionResult All();

        IActionResult AllByIdentifier(string currencySlug, string currencyPairGuid, string currencyTypeShortForm,
            int index = 0, int itemsPerPage = 200);
        
        IActionResult Create(CreateAnalysedComponentViewModel vm);

        IActionResult Get(string guid);
    }
}
