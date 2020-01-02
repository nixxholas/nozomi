using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.ComponentType
{
    public interface IComponentTypeController
    {
        IActionResult All();
    }
}