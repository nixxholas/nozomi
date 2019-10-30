using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.RequestComponent;

namespace Nozomi.Web.Controllers.APIs.v1.RequestComponent
{
    public interface IRequestComponentController
    {
        IActionResult Create(CreateRequestComponentViewModel vm);
    }
}
