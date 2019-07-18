using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.Request
{
    [ApiController]
    public class RequestController : BaseController<RequestController>, IRequestController
    {
        private readonly IRequestEvent _requestEvent;

        public RequestController(ILogger<RequestController> logger, NozomiUserManager userManager,
            IRequestEvent requestEvent) : base(logger, userManager)
        {
            _requestEvent = requestEvent;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All(bool includeNested)
        {
            var res = _requestEvent.GetAllActive();

            return new NozomiResult<JsonResult>()
            {
                ResultType = res != null ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = res != null ? new JsonResult(res) :
                    new JsonResult("We're currently unable to obtain all of the Currency Pair Requests.")
            };
        }
    }
}
