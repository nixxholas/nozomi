using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.Models.Web;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPairComponent
{
    public class CurrencyPairComponentController : BaseController<CurrencyPairComponentController>, ICurrencyPairComponentController
    {
        private readonly IRequestComponentEvent _requestComponentEvent;
        private readonly IRequestComponentService _requestComponentService;
        
        public CurrencyPairComponentController(ILogger<CurrencyPairComponentController> logger, UserManager<User> userManager,
            IRequestComponentEvent requestComponentEvent, IRequestComponentService requestComponentService) 
            : base(logger, userManager)
        {
            _requestComponentEvent = requestComponentEvent;
            _requestComponentService = requestComponentService;
        }

        [HttpGet]
        //[SwaggerResponse(200, "Request components obtained.", typeof(NozomiResult<ICollection<Component>>))]
        //[SwaggerResponse(400, "The request ID is invalid.")]
        [SwaggerOperation(
            Summary = "Obtains all Request Components related to the specific Request ID."
        )]
        public NozomiResult<ICollection<Component>> AllByRequestId(long requestId, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Component>>
                (_requestComponentEvent.GetAllByRequest(requestId, includeNested));
        }

        [HttpGet]
        public NozomiResult<ICollection<Component>> All(int index = 0, bool includeNested = false)
        {
            return new NozomiResult<ICollection<Component>>
                (_requestComponentEvent.All(index, includeNested));
        }

        [Authorize]
        [HttpPost]
        public NozomiResult<string> Create(CreateRequestComponent createRequestComponent)
        {
            return _requestComponentService.Create(createRequestComponent);
        }

        [Authorize]
        [HttpPost]
        public NozomiResult<string> Update(UpdateRequestComponent updateRequestComponent)
        {
            return _requestComponentService.Update(updateRequestComponent);
        }

        [Authorize]
        [HttpDelete]
        public NozomiResult<string> Delete(long id, string userId = null, bool hardDelete = false)
        {
            return _requestComponentService.Delete(id, userId, hardDelete);
        }
    }
}