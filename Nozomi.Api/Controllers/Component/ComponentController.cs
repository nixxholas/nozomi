using System.Collections.Generic;
using System.Net;
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

        public IActionResult Get(string guid, int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}