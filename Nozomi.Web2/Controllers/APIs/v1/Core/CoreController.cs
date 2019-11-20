using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;

namespace Nozomi.Web2.Controllers.APIs.v1.Core
{
    public class CoreController : BaseApiController<CoreController>, ICoreController
    {
        public CoreController(ILogger<CoreController> logger) : base(logger)
        {
        }

        [HttpGet]
        public DateTime GetCurrentBuildTime()
        {
            return CoreConstants.BuildDateTime;
        }
    }
}
