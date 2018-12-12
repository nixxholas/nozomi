using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Ticker.Areas.v1.RequestType
{
    public class RequestTypeController : BaseController<RequestTypeController>, IRequestTypeController
    {
        private IRequestTypeService _requestTypeService;
        
        public RequestTypeController(ILogger<RequestTypeController> logger,
            IRequestTypeService requestTypeService) : base(logger)
        {
            _requestTypeService = requestTypeService;
        }

        [HttpGet]
        public NozomiResult<JsonResult> All()
        {
            return new NozomiResult<JsonResult>(new JsonResult(_requestTypeService.All()));
        }
    }
}