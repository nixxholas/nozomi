using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Nozomi.Web2.Controllers.v1.CurrencyPairComponent
{
    public class CurrencyPairComponentController : BaseApiController<CurrencyPairComponentController>,
        ICurrencyPairComponentController
    {
        private readonly IComponentEvent _componentEvent;

        public CurrencyPairComponentController(ILogger<CurrencyPairComponentController> logger,
            IComponentEvent componentEvent)
            : base(logger)
        {
            _componentEvent = componentEvent;
        }

        [HttpGet]
        //[SwaggerResponse(200, "Request components obtained.", typeof(NozomiResult<ICollection<Component>>))]
        //[SwaggerResponse(400, "The request ID is invalid.")]
        [SwaggerOperation(
            Summary = "Obtains all Request Components related to the specific Request ID."
        )]
        public NozomiResult<ICollection<Data.Models.Web.Component>> AllByRequestId(long requestId, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Data.Models.Web.Component>>
                (_componentEvent.GetAllByRequest(requestId, includeNested));
        }

        [HttpGet]
        public NozomiResult<ICollection<Data.Models.Web.Component>> All(int index = 0, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Data.Models.Web.Component>>
                (_componentEvent.All(index, includeNested));
        }
    }
}
