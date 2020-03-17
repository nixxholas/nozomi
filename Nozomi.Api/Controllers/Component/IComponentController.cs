using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.Component
{
    public interface IComponentController
    {
        IActionResult All(int index = 0, string requestGuid = null);

        IActionResult Get(string guid, int index = 0);
    }
}