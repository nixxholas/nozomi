using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponentType
{
    public class AnalysedComponentTypeController : BaseApiController<AnalysedComponentTypeController>,
        IAnalysedComponentTypeController
    {
        public AnalysedComponentTypeController(ILogger<AnalysedComponentTypeController> logger) : base(logger)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Throttle(Name = "AnalysedComponentType/All", Milliseconds = 100)]
        public IActionResult All()
        {
            return Ok(NozomiServiceConstants.AnalysedComponentTypeMap);
        }
    }
}