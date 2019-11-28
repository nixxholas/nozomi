using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing;

namespace Nozomi.Web2.Controllers.v1.AnalysedComponentType
{
    public class AnalysedComponentTypeController : BaseApiController<AnalysedComponentTypeController>,
        IAnalysedComponentTypeController
    {
        public AnalysedComponentTypeController(ILogger<AnalysedComponentTypeController> logger) : base(logger)
        {
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(NozomiServiceConstants.AnalysedComponentTypeMap);
        }
    }
}