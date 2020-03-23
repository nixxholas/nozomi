using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.ComponentType.Examples;
using Nozomi.Data.ViewModels.ComponentType;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.ComponentType
{
    /// <summary>
    /// Component Type APIs
    /// </summary>
    public class ComponentTypeController : BaseApiController<ComponentTypeController>, IComponentTypeController
    {
        private readonly IComponentTypeEvent _componentTypeEvent;
        private readonly INozomiRedisEvent _nozomiRedisEvent;

        /// <summary>
        /// Default Constructor..
        /// </summary>
        /// <param name="logger">Logger DI</param>
        /// <param name="componentTypeEvent">Component Type Event DI</param>
        /// <param name="nozomiRedisEvent">Nozomi Redis Event DI</param>
        public ComponentTypeController(ILogger<ComponentTypeController> logger,
            IComponentTypeEvent componentTypeEvent, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _componentTypeEvent = componentTypeEvent;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        /// <summary>
        /// Obtain all of the component types that are publicly available or the ones that you own.
        /// </summary>
        /// <param name="index">the 'page' of the list of results in 50s.</param>
        /// <returns>Collection of component types.</returns>
        [Authorize]
        [TokenBucket(Name = "ComponentType/All", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ComponentTypeViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public async Task<IActionResult> All([FromQuery]int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_componentTypeEvent.ViewAll(userId, index));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }
    }
}