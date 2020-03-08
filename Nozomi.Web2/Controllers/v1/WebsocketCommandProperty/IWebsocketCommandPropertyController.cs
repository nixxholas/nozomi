using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Web2.Controllers.v1.WebsocketCommandProperty
{
    public interface IWebsocketCommandPropertyController
    {
        IActionResult Get(string guid);

        IActionResult GetByCommand(string commandGuid);
        
        IActionResult Create(CreateWebsocketCommandPropertyInputModel vm);
        
        IActionResult Update(UpdateWebsocketCommandPropertyInputModel vm);

        IActionResult Delete(string websocketCommandPropertyGuid);
    }
}