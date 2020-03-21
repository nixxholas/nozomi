using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.Connect.Examples;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Options;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Connect
{
    /// <summary>
    /// The Connect APIs, to enable/provide certain numerous functions for users.
    /// </summary>
    public class ConnectController : BaseApiController<ConnectController>, IConnectController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public ConnectController(ILogger<ConnectController> logger, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
        }
        
        /// <summary>
        /// Allows the client to validate his API key.
        /// </summary>
        /// <returns>Result of the validation</returns>
        [Authorize]
        [Throttle(Name = "Connect/Validate", Milliseconds = 2500)]
        [HttpHead]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ValidateOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(ValidateBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(ValidateInternalServerExample))]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);

                if (userId.HasValue)
                    return Ok(ValidateOkExample.Result);
                else
                    return BadRequest(ValidateBadRequestExample.Result);
            }

            return BadRequest(ValidateInternalServerExample.Result);
        }
    }
}