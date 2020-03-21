using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.AnalysedComponent.Examples;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Analysis.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.AnalysedComponent
{
    public class AnalysedComponentController : BaseApiController<AnalysedComponentController>,
        IAnalysedComponentController
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger,
            IAnalysedComponentEvent analysedComponentEvent, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        [Authorize]
        [TokenBucket(Name = "AnalysedComponent/All", Weight = 5)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AnalysedComponentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All([FromQuery]int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_analysedComponentEvent.ViewAll(index, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }

        [Authorize]
        [TokenBucket(Name = "AnalysedComponent/AllByIdentifier", Weight = 7)]
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AnalysedComponentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult AllByIdentifier(string currencySlug = null, string tickerPair = null, 
            string currencyTypeShortForm = null, int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_analysedComponentEvent.ViewAllByIdentifier(currencySlug, tickerPair, currencyTypeShortForm, 
                    index,userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }

        [Authorize]
        [TokenBucket(Name = "AnalysedComponent/Get", Weight = 7)]
        [HttpGet("{guid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AnalysedComponentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GetOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(GetBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(GetInternalServerExample))]
        public IActionResult Get(string guid, int index = 0)
        {
            if (Guid.TryParse(guid, out var parsedGuid) && index >= 0)
            {
                if (!HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey,
                    out var apiKey))
                    return new InternalServerErrorObjectResult(GetInternalServerExample.Result);
                
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                if (string.IsNullOrEmpty(userId))
                    return new InternalServerErrorObjectResult(GetInternalServerExample.Result);
                
                return Ok(_analysedComponentEvent.View(parsedGuid, index, userId));
            }

            return BadRequest(GetBadRequestExample.Result);
        }
    }
}