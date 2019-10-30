using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Web.Controllers.APIs.v1.AnalysedComponent
{
    public interface IAnalysedComponentController
    {
        IActionResult Create(CreateAnalysedComponentViewModel vm);
    }
}
