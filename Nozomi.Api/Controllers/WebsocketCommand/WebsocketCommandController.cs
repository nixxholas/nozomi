using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.WebsocketCommand.Examples;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommand
{
    /// <summary>
    /// Websocket Command APIs
    /// </summary>
    public class WebsocketCommandController : BaseApiController<WebsocketCommandController>, 
        IWebsocketCommandController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IWebsocketCommandEvent _websocketCommandEvent;

        /// <summary></summary>
        /// <param name="logger">Logger DI</param>
        /// <param name="nozomiRedisEvent">Redis Cache Events DI</param>
        /// <param name="websocketCommandEvent">Websocket Command Events DI</param>
        public WebsocketCommandController(ILogger<WebsocketCommandController> logger, 
            INozomiRedisEvent nozomiRedisEvent, IWebsocketCommandEvent websocketCommandEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _websocketCommandEvent = websocketCommandEvent;
        }

        /// <summary>
        /// Obtain all websocket commands you have created/own.
        /// </summary>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <returns>The collection of websocket commands.</returns>
        [Authorize]
        [TokenBucket(Name = "WebsocketCommand/All", Weight = 7)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<WebsocketCommandViewModel>))]
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
                    
                return Ok(_websocketCommandEvent.ViewAll(index, null, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.ImpossibleInvalidUserResult);
        }

        /// <summary>
        /// Obtain all of the websocket commands created, relative to the request.
        /// </summary>
        /// <param name="requestGuid">The unique identifier of the request.</param>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <returns>The collection of websocket commands.</returns>
        [Authorize]
        [TokenBucket(Name = "WebsocketCommand/AllByRequest", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<WebsocketCommandViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllByRequestOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllByRequestBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllByRequestInternalServerExample))]
        public IActionResult AllByRequest(string requestGuid, int index = 0)
        {
            if (index >= 0) return BadRequest(AllByRequestBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                
                if (!Guid.TryParse(userId, out var parsedGuid)) return new InternalServerErrorObjectResult(
                    AllByRequestInternalServerExample.InvalidUserResult);
                    
                return Ok(_websocketCommandEvent.ViewAll(index, requestGuid, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllByRequestInternalServerExample.ImpossibleInvalidUserResult);
        }
    }
}