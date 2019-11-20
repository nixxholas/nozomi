using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Web2.Controllers.APIs.v1.Component
{
    public interface IComponentController
    {
        IActionResult Create(CreateComponentViewModel vm);
    }
}
