using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Web2.Controllers.v1.Component
{
    public interface IComponentController
    {
        IActionResult AllByRequest(string requestGuid, int index = 0, int itemsPerPage = 50,
            bool includeNested = false);
        
        IActionResult All(int index = 0, int itemsPerPage = 50, bool includeNested = false);
        
        IActionResult Create(CreateComponentViewModel vm);
    }
}
