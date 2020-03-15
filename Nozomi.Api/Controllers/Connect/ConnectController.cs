using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Api.Controllers.Connect
{
    public class ConnectController : BaseApiController<ConnectController>, IConnectController
    {
        public ConnectController(ILogger<ConnectController> logger) : base(logger)
        {
        }

        [Throttle(Name = "Connect/Validate", Milliseconds = 2500)]
        [HttpHead]
        public IActionResult Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}