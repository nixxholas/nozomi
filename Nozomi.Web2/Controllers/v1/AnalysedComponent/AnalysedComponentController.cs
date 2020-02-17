using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponent
{
    public class AnalysedComponentController : BaseApiController<AnalysedComponentController>,
        IAnalysedComponentController
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly IAnalysedComponentTypeEvent _analysedComponentTypeEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger,
            IAnalysedComponentEvent analysedComponentEvent, IAnalysedComponentTypeEvent analysedComponentTypeEvent,
            IAnalysedComponentService analysedComponentService) : base(logger)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _analysedComponentTypeEvent = analysedComponentTypeEvent;
            _analysedComponentService = analysedComponentService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            var payload = _analysedComponentTypeEvent.GetAllKeyValuePairs();

            if (payload == null)
                return StatusCode(500);

            return Ok(payload);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AllByIdentifier([FromQuery]string currencySlug, [FromQuery]string currencyPairGuid, 
            [FromQuery]string currencyTypeShortForm, [FromQuery]int index = 0, [FromQuery]int itemsPerPage = 200)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;
                
            // Obtain the user's roles
            var role = ((ClaimsIdentity) User.Identity).Claims
                .Where(c => c.Type.Equals(JwtClaimTypes.Role));
                
            // If the user is a staff, let him access the entity indefinitely
            if (role.Any(r => NozomiPermissions.AllStaffRoles
                .Any(sr => sr.GetDescription().Equals(r.Value))))
                return Ok(_analysedComponentEvent.All(currencySlug, currencyPairGuid, currencyTypeShortForm, index,
                    itemsPerPage));

            if (!string.IsNullOrWhiteSpace(sub))
            {
                return Ok(_analysedComponentEvent.All(currencySlug, currencyPairGuid, currencyTypeShortForm, index,
                    itemsPerPage, sub));
            }

            return BadRequest("Please login again. Your session may have expired!");
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        public IActionResult Create([FromBody]CreateAnalysedComponentViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _analysedComponentService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please login again. Your session may have expired!");
        }

        [Authorize]
        [HttpGet("{guid}")]
        public IActionResult Get(string guid)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub) && Guid.TryParse(guid, out var acGuid))
            {
                return Ok(_analysedComponentEvent.Get(acGuid, sub));
            }

            return BadRequest("Please login again. Your session may have expired!");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Update([FromBody]UpdateAnalysedComponentViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _analysedComponentService.Update(vm, sub);

                return Ok();
            }

            return BadRequest("Please login again. Your session may have expired!");
        }
    }
}
