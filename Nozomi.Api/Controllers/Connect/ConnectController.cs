using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class ConnectController : BaseApiController<ConnectController>, IConnectController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public ConnectController(ILogger<ConnectController> logger, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        [Authorize]
        [Throttle(Name = "Connect/Validate", Milliseconds = 2500)]
        [HttpHead]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ValidateOkExample))]
        // [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
        
        protected class ValidateOkExample : IExamplesProvider<string>
        {
            public const string Result = "Hey you're legit!";
            
            public string GetExamples()
            {
                return Result;
            }
        }

        protected class ValidateBadRequestExample : IExamplesProvider<string>
        {
            public const string Result = "Invalid API Key.";
            
            public string GetExamples()
            {
                return Result;
            }
        }

        protected class ValidateInternalServerExample : IExamplesProvider<string>
        {
            public const string Result = "Where's your API Key mate?";

            public string GetExamples()
            {
                return Result;
            }
        }
    }
}