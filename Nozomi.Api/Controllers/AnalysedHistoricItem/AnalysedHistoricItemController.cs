using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Api.Controllers.AnalysedHistoricItem.Examples;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Preprocessing.Options;
using Nozomi.Service.Events.Analysis.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.AnalysedHistoricItem
{
    public class AnalysedHistoricItemController : BaseApiController<AnalysedHistoricItemController>, 
        IAnalysedHistoricItemController
    {
        private readonly IAnalysedHistoricItemEvent _analysedHistoricItemEvent;
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        
        public AnalysedHistoricItemController(ILogger<AnalysedHistoricItemController> logger,
            IAnalysedHistoricItemEvent analysedHistoricItemEvent, INozomiRedisEvent nozomiRedisEvent) : base(logger)
        {
            _analysedHistoricItemEvent = analysedHistoricItemEvent;
            _nozomiRedisEvent = nozomiRedisEvent;
        }

        /// <summary>
        /// Allows the client to retrieve all historical values of the mentioned analysed component.
        /// </summary>
        /// <param name="acGuid">The time this value was generated.</param>
        /// <param name="index">The value of the analysed component at the said timestamp.</param>
        /// <returns>Returns the collection of historic values of the aforementioned Analysed component.</returns>
        [Authorize]
        [TokenBucket(Name = "AnalysedHistoricItem/All", Weight = 5)]
        [HttpGet("{acGuid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<AnalysedHistoricItemViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AllOkExample))]
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(AllBadRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(AllInternalServerExample))]
        public IActionResult All(string acGuid = null, [FromQuery]int index = 0)
        {
            if (index >= 0) return BadRequest(AllBadRequestExample.Result);
            
            if (HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                
                return Ok(_analysedHistoricItemEvent.List(Guid.Parse(userId), index));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult(AllInternalServerExample.Result);
        }
    }
}