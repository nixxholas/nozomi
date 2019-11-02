using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityModel;
using Nozomi.Data.ResponseModels.SourceType;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.SourceType
{
    public class SourceTypeController : BaseApiController<SourceTypeController>, ISourceTypeController
    {
        private readonly ISourceTypeEvent _sourceTypeEvent;
        private readonly ISourceTypeService _sourceTypeService;

        public SourceTypeController(ILogger<SourceTypeController> logger, ISourceTypeEvent sourceTypeEvent,
            ISourceTypeService sourceTypeService) : base(logger)
        {
            _sourceTypeEvent = sourceTypeEvent;
            _sourceTypeService = sourceTypeService;
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_sourceTypeEvent.GetAll());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateSourceTypeViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _sourceTypeService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}
