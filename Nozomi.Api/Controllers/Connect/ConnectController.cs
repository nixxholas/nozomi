using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Attributes;

namespace Nozomi.Api.Controllers.Connect
{
    public class ConnectController : BaseApiController<ConnectController>, IConnectController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public ConnectController(ILogger<ConnectController> logger, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        [Throttle(Name = "Connect/Validate", Milliseconds = 2500)]
        [HttpHead]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);

                if (userId.HasValue)
                    return Ok();
                else
                    return BadRequest("Invalid API Key.");
            }

            return BadRequest("Please inject the API key in the authorization header.");
        }
    }
}