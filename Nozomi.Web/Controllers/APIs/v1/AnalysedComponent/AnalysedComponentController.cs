using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.AnalysedComponent
{
    public class AnalysedComponentController : BaseApiController<AnalysedComponentController>,
        IAnalysedComponentController
    {
        private readonly IAnalysedComponentService _analysedComponentService;
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger,
            IAnalysedComponentService analysedComponentService) : base(logger)
        {
            _analysedComponentService = analysedComponentService;
        }

        [Authorize]
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
