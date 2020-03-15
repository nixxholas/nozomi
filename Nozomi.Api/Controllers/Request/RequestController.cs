using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Api.Controllers.Request
{
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IRequestEvent _requestEvent;

        public RequestController(ILogger<RequestController> logger,
            INozomiRedisEvent nozomiRedisEvent, IRequestEvent requestEvent) : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _requestEvent = requestEvent;
        }

        /// <summary>
        /// Retrieves all requests owned by the stated user with a pagination of 100 items.
        /// </summary>
        /// <param name="index">The 'page' of the request list you're viewing</param>
        /// <returns>Requests</returns>
        /// <response code="200">All requests obtained</response>
        /// <response code="400">Your request is either unauthorised or has exceeded the index limits</response>
        /// <response code="500">Not sure how you got here, but no.</response>
        [TokenBucket(Name = "Request/Get", Weight = 5)]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<RequestViewModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult All(int index = 0)
        {
            if (index >= 0) return BadRequest("Invalid index.");
            
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);
                    
                return Ok(_requestEvent.All(index, userId));
            }

            _logger.LogWarning($"{_controllerName} All: User managed to bypass the token bucket " +
                               "attribute without an API key!");
            return new InternalServerErrorObjectResult("Not sure how you got here, but no.");
        }

        /// <summary>
        /// Retrieves the request with the mentioned guid.
        /// </summary>
        /// <param name="guid">Guid of the request</param>
        /// <returns>Request</returns>
        /// <response code="200">Request obtained</response>
        /// <response code="400">Can't really find the request you're trying to obtain...</response>
        /// <response code="500">Not sure how you got here, but no.</response>
        [TokenBucket(Name = "Request/Get", Weight = 1)]
        [HttpGet("{guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RequestViewModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Get(string guid)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
                    return Ok(_requestEvent.GetByGuid(parsedGuid, 
                        _nozomiRedisEvent.GetValue(apiKey).ToString()));

                _logger.LogWarning($"{_controllerName} Get: User managed to bypass the token bucket " +
                                   "attribute without an API key!");
                return new InternalServerErrorObjectResult("Not sure how you got here, but no.");
            }

            return BadRequest("Invalid Guid.");
        }
    }
}