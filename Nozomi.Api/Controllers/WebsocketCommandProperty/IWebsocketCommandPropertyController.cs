using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty
{
    public interface IWebsocketCommandPropertyController
    {
        IActionResult All(int index = 0);
        
        IActionResult AllByCommand(string commandGuid, int index = 0);
    }
}