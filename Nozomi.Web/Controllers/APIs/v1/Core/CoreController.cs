using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Web.Controllers.APIs.v1.Core
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
