using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Dispatch;

namespace Nozomi.Web2.Controllers.v1.Dispatch
{
    public interface IDispatchController
    {
        Task<IActionResult> Fetch(DispatchInputModel vm);
    }
}