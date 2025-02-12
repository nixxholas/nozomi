using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.RequestProperty.Examples;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.RequestProperty
{
    /// <summary>
    /// Request Property APIs
    /// </summary>
    public class RequestPropertyController : BaseApiController<RequestPropertyController>, IRequestPropertyController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IRequestPropertyEvent _requestPropertyEvent;
        
        /// <summary></summary>
        /// <param name="logger">Logger DI</param>
        /// <param name="nozomiRedisEvent"></param>
        /// <param name="requestPropertyEvent"></param>
        public RequestPropertyController(ILogger<RequestPropertyController> logger,
            INozomiRedisEvent nozomiRedisEvent, IRequestPropertyEvent requestPropertyEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _requestPropertyEvent = requestPropertyEvent;
        }

        /// <summary>
        /// Obtain all request properties you have created/own.
        /// </summary>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <returns>The collection of request properties.</returns>
        [Authorize]
        [TokenBucket(Name = "RequestProperty/All", Weight = 7)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<RequestPropertyViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All(int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.InvalidIndexResult);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                
                if (!Guid.TryParse(userId, out var parsedGuid)) return new InternalServerErrorObjectResult(
                    AllInternalServerExample.InvalidUserResult);
                    
                return Ok(_requestPropertyEvent.ViewAll(index, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.ImpossibleInvalidUserResult);
        }

        /// <summary>
        /// Obtain all analysed components you have created, relative to that specific request.
        /// </summary>
        /// <param name="requestGuid">The request guid you are referring to.</param>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <returns>The collection of request properties.</returns>
        [Authorize]
        [TokenBucket(Name = "RequestProperty/AllByRequest", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<RequestPropertyViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllByRequestOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllByRequestBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllByRequestInternalServerExample))]
        public IActionResult AllByRequest(string requestGuid, int index = 0)
        {
            if (index >= 0) return BadRequest(AllByRequestBadRequestExample.InvalidIndexResult);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                
                if (!Guid.TryParse(userId, out var parsedGuid)) return new InternalServerErrorObjectResult(
                    AllByRequestInternalServerExample.InvalidUserResult);
                    
                return Ok(_requestPropertyEvent.ViewByRequest(requestGuid, index, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllByRequestInternalServerExample.ImpossibleInvalidUserResult);
        }
    }
}