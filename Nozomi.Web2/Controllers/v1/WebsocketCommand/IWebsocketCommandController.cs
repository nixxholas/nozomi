using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommand
{
    public interface IWebsocketCommandController
    {
        IActionResult Get(string guid);

        IActionResult GetByRequest(string requestGuid);
        
        IActionResult Create(CreateWebsocketCommandInputModel vm);
        
        IActionResult Update(UpdateWebsocketCommandInputModel vm);

        IActionResult Delete(string websocketCommandGuid);
    }
}