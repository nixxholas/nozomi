using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.ComponentHistoricItem.Examples;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.ComponentHistoricItem
{
    /// <summary>
    /// ComponentHistoricItem APIs.
    /// </summary>
    public class ComponentHistoricItemController : BaseApiController<ComponentHistoricItemController>, 
        IComponentHistoricItemController
    {
        private readonly IComponentHistoricItemEvent _componentHistoricItemEvent;
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        /// <summary>
        /// Constructor with DI elements.
        /// </summary>
        /// <param name="logger">ILogger object.</param>
        /// <param name="componentHistoricItemEvent">Where we usually call functions to perform ComponentHistoricItem
        /// actions.</param>
        /// <param name="nozomiRedisEvent">Redis cache read access events.</param>
        public ComponentHistoricItemController(ILogger<ComponentHistoricItemController> logger,
            IComponentHistoricItemEvent componentHistoricItemEvent, INozomiRedisEvent nozomiRedisEvent) 
            : base(logger)
        {
            _componentHistoricItemEvent = componentHistoricItemEvent;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        /// <summary>
        /// Obtain all the component historical values created 
        /// </summary>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <param name="componentGuid">The unique identifier of the component.</param>
        /// <returns>The collection of historical values.</returns>
        [Authorize]
        [TokenBucket(Name = "ComponentHistoricItem/All", Weight = 5)]
        [HttpGet("{componentGuid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<ComponentHistoricItemViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All(int index = 0, string componentGuid = null)
        {
            if (index < 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);

                if (!Guid.TryParse(userId, out var parsedGuid)) return BadRequest();
                
                return Ok(_componentHistoricItemEvent.ViewAll(index, componentGuid, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }
    }
}