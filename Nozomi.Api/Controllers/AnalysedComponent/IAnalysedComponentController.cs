using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.AnalysedComponent
{
    public interface IAnalysedComponentController
    {
        IActionResult All(int index = 0);

        IActionResult AllByIdentifier(string currencySlug, string currencyPairGuid, string currencyTypeShortForm,
            int index = 0, int itemsPerPage = 200);
        
        IActionResult Get(string guid);
    }
}