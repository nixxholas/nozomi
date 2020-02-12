using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Web2.Controllers.v1.RequestProperty
{
    public interface IRequestPropertyController
    {
        IActionResult Get(string guid);

        IActionResult GetAllByRequest(string requestGuid);
        
        Task<IActionResult> Create(CreateRequestPropertyInputModel vm);

        Task<IActionResult> Update(UpdateRequestPropertyInputModel vm);

        Task<IActionResult> Delete(string guid);
    }
}