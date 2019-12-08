using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Web.Controllers.APIs.v1.Component
{
    public interface IComponentController
    {
        IActionResult Create(CreateComponentViewModel vm);
    }
}
