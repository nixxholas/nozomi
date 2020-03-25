using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Dispatch;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Dispatch
{
    public class DispatchController : BaseApiController<DispatchController>, IDispatchController
    {
        private readonly IDispatchEvent _dispatchEvent;
        
        public DispatchController(ILogger<DispatchController> logger, IDispatchEvent dispatchEvent) : base(logger)
        {
            _dispatchEvent = dispatchEvent;
        }

        public Task<IActionResult> Dispatch(DispatchInputModel vm)
        {
            throw new System.NotImplementedException();
        }
    }
}