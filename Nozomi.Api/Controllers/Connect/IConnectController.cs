using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.Connect
{
    public interface IConnectController
    {
        IActionResult Validate();
    }
}