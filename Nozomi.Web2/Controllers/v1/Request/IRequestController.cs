using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Request;

namespace Nozomi.Web2.Controllers.v1.Request
{
    public interface IRequestController
    {
        NozomiResult<JsonResult> All(bool includeNested);

        IActionResult Create(CreateRequestViewModel vm);
        
        IActionResult GetAll(int index = 0);

        IActionResult Update(UpdateRequestViewModel vm);

        IActionResult Delete(string guid);
    }
}
