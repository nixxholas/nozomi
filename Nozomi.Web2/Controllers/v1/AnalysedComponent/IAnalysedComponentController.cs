using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponent
{
    public interface IAnalysedComponentController
    {
        IActionResult All();
        
        IActionResult Create(CreateAnalysedComponentViewModel vm);

        IActionResult Get(string guid);
    }
}
