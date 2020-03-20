using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.WebsocketCommand
{
    public interface IWebsocketCommandController
    {
        IActionResult All(int index = 0);
        
        IActionResult AllByRequest(string requestGuid, int index = 0);
    }
}