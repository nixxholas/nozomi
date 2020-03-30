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
    [SwaggerTag("API Set that provides interaction with Analysed Components.")]
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

        /// <summary>
        /// Obtain all analysed components you have created. 
        /// </summary>
        /// <param name="index">the 'page' of the list of results in 100s.</param>
        /// <returns>The collection of analysed components.</returns>
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

        /// <summary>
        /// Obtain all of the analysed components created, related to that specific entity (if any).
        /// </summary>
        /// <param name="currencySlug">The currency slug you are referring to.</param>
        /// <param name="tickerPair">The ticker pair you are referring to.</param>
        /// <param name="currencyTypeShortForm">The currency type's short form you are referring to.</param>
        /// <param name="index">The 'page' of the list of results in 100s.</param>
        /// <returns>The collection of analysed components.</returns>
        [Obsolete]
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

        /// <summary>
        /// Obtain the analysed component and its historical values.
        /// </summary>
        /// <param name="guid">The unique identifier of the analysed component.</param>
        /// <param name="index">The 'page' of the list of historical values in 500s.</param>
        /// <returns>The analysed component.</returns>
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