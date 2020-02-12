using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.RequestProperty
{
    public class RequestPropertyController : BaseApiController<RequestPropertyController>, IRequestPropertyController
    {
        private readonly IRequestPropertyEvent _requestPropertyEvent;
        private readonly IRequestPropertyService _requestPropertyService;
        
        public RequestPropertyController(ILogger<RequestPropertyController> logger,
            IRequestPropertyEvent requestPropertyEvent, IRequestPropertyService requestPropertyService) : base(logger)
        {
            _requestPropertyEvent = requestPropertyEvent;
            _requestPropertyService = requestPropertyService;
        }

        [Authorize]
        [HttpGet("{guid}")]
        public IActionResult Get(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrEmpty(guid))
                return Ok(_requestPropertyEvent.GetByGuid(guid, sub));

            if (!string.IsNullOrWhiteSpace(sub) && string.IsNullOrEmpty(guid))
                return BadRequest("Please enter a valid ID!");
            return BadRequest("Please re-authenticate again");
        }

        [Authorize]
        [HttpGet("{requestGuid}")]
        public IActionResult GetAllByRequest(string requestGuid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            // Since we get the sub,
            if (!string.IsNullOrWhiteSpace(sub) && !string.IsNullOrEmpty(requestGuid))
                return Ok(_requestPropertyEvent.GetByRequest(requestGuid, sub));

            if (!string.IsNullOrWhiteSpace(sub) && string.IsNullOrEmpty(requestGuid))
                return BadRequest("Please enter a valid ID!");
            return BadRequest("Please re-authenticate again");
        }

        public IActionResult Create(CreateRequestPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Update(UpdateRequestPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Delete(string guid)
        {
            throw new System.NotImplementedException();
        }
    }
}