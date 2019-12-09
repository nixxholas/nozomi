using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;

namespace Nozomi.Web2.Controllers.v1.Core
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
