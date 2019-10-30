using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.Models.Web;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyPairComponent
{
    public class CurrencyPairComponentController : BaseApiController<CurrencyPairComponentController>,
        ICurrencyPairComponentController
    {
        private readonly IRequestComponentEvent _requestComponentEvent;

        public CurrencyPairComponentController(ILogger<CurrencyPairComponentController> logger,
            IRequestComponentEvent requestComponentEvent)
            : base(logger)
        {
            _requestComponentEvent = requestComponentEvent;
        }

        [HttpGet]
        //[SwaggerResponse(200, "Request components obtained.", typeof(NozomiResult<ICollection<RequestComponent>>))]
        //[SwaggerResponse(400, "The request ID is invalid.")]
        [SwaggerOperation(
            Summary = "Obtains all Request Components related to the specific Request ID."
        )]
        public NozomiResult<ICollection<Data.Models.Web.RequestComponent>> AllByRequestId(long requestId, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Data.Models.Web.RequestComponent>>
                (_requestComponentEvent.GetAllByRequest(requestId, includeNested));
        }

        [HttpGet]
        public NozomiResult<ICollection<Data.Models.Web.RequestComponent>> All(int index = 0, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Data.Models.Web.RequestComponent>>
                (_requestComponentEvent.All(index, includeNested));
        }
    }
}
