using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.AnalysedComponent
{
    public interface IAnalysedComponentController
    {
        IActionResult All(int index = 0);

        IActionResult AllByIdentifier(string currencySlug = null, string tickerPair = null, 
            string currencyTypeShortForm = null, int index = 0);
        
        IActionResult Get(string guid);
    }
}