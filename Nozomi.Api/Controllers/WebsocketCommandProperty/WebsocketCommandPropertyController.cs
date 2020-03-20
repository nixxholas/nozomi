using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.WebsocketCommandProperty.Examples;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty
{
    public class WebsocketCommandPropertyController : BaseApiController<WebsocketCommandPropertyController>,
        IWebsocketCommandPropertyController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IWebsocketCommandPropertyEvent _websocketCommandPropertyEvent;
        
        public WebsocketCommandPropertyController(ILogger<WebsocketCommandPropertyController> logger,
            INozomiRedisEvent nozomiRedisEvent, IWebsocketCommandPropertyEvent websocketCommandPropertyEvent) 
            : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
        }

        [Authorize]
        [TokenBucket(Name = "WebsocketCommandProperty/All", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<WebsocketCommandPropertyViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All(int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                
                if (!Guid.TryParse(userId, out var parsedGuid)) return new InternalServerErrorObjectResult(
                    AllInternalServerExample.InvalidUserResult);
                    
                return Ok(_websocketCommandPropertyEvent.ViewAll(index, null, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.ImpossibleInvalidUserResult);
        }

        public IActionResult AllByCommand(string commandGuid, int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}