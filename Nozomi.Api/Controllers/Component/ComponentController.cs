using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.Component.Examples;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Component
{
    public class ComponentController : BaseApiController<ComponentController>, IComponentController
    {
        private readonly IComponentEvent _componentEvent;
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public ComponentController(ILogger<ComponentController> logger, IComponentEvent componentEvent,
            INozomiRedisEvent nozomiRedisEvent) 
            : base(logger)
        {
            _componentEvent = componentEvent;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        /// <summary>
        /// Obtain all components you have created.
        /// </summary>
        /// <param name="index">The 'page' of the list of results in 100s</param>
        /// <param name="requestGuid">The unique identifier of the request that contains this component.</param>
        /// <returns>The collection of components.</returns>
        [Authorize]
        [TokenBucket(Name = "Component/All", Weight = 5)]
        [HttpGet("{requestGuid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<ComponentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All(int index = 0, string requestGuid = null)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_componentEvent.All(index, includeNested: true, userId: userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }

        /// <summary>
        /// Obtain the component and its historical values.
        /// </summary>
        /// <param name="guid">The unique identifier of the component.</param>
        /// <param name="index">The 'page' of the list of historical values in 100s</param>
        /// <returns>The component.</returns>
        [Authorize]
        [TokenBucket(Name = "Component/Get", Weight = 5)]
        [HttpGet("{guid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ComponentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(GetBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(GetInternalServerExample))]
        public IActionResult Get(string guid, int index = 0)
        {
            if (index < 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_componentEvent.View(guid, index, userId));
            }

            _logger.LogWarning($"{_controllerName} Get: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }
    }
}