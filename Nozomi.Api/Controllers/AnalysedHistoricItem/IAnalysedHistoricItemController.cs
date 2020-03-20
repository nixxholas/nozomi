using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.AnalysedHistoricItem
{
    public interface IAnalysedHistoricItemController
    {
        IActionResult All(string acGuid = null, int index = 0);
    }
}