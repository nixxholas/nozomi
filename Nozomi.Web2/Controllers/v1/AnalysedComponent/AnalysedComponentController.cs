using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponent
{
    public class AnalysedComponentController : BaseApiController<AnalysedComponentController>,
        IAnalysedComponentController
    {
        private readonly IAnalysedComponentTypeEvent _analysedComponentTypeEvent;
        private readonly IAnalysedComponentService _analysedComponentService;
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger,
            IAnalysedComponentTypeEvent analysedComponentTypeEvent,
            IAnalysedComponentService analysedComponentService) : base(logger)
        {
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
    }
}
