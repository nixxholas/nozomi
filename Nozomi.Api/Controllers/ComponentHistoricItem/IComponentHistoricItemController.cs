using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.ComponentHistoricItem
{
    public interface IComponentHistoricItemController
    {
        IActionResult All(int index = 0);

        IActionResult Get(string guid, int index = 0);
    }
}